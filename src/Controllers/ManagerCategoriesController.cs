using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.Models;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("manager/categories")]
public class ManagerCategoriesController : Controller
{
    private readonly AppDbContext _dbContext;

    public ManagerCategoriesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "categories";
        ViewData["Title"] = "Quản lý danh mục";

        var categories = await _dbContext.Categories
            .Where(c => c.DeletedAt == null)
            .OrderBy(c => c.Order)
            .ThenBy(c => c.Name)
            .ToListAsync();

        return View(categories);
    }

    [HttpGet("action")]
    public async Task<IActionResult> Action(string? id)
    {
        ViewData["ActiveMenu"] = "categories";

        if (string.IsNullOrEmpty(id))
        {
            ViewData["Title"] = "Thêm danh mục";
            return View(new CategoryFormViewModel());
        }

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category is null)
            return NotFound();

        ViewData["Title"] = "Sửa danh mục";
        return View(new CategoryFormViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Color = category.Color,
            Description = category.Description,
            Order = category.Order,
            Status = category.Status
        });
    }

    [HttpPost("action")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Action(CategoryFormViewModel model, string? id)
    {
        ViewData["ActiveMenu"] = "categories";
        ViewData["Title"] = string.IsNullOrEmpty(id) ? "Thêm danh mục" : "Sửa danh mục";

        if (!ModelState.IsValid)
            return View(model);

        if (string.IsNullOrEmpty(id))
        {
            var category = new Category
            {
                Name = model.Name.Trim(),
                Slug = await GenerateUniqueSlugAsync(model.Name),
                Color = model.Color?.Trim(),
                Description = model.Description?.Trim(),
                Order = model.Order,
                Status = model.Status
            };
            _dbContext.Categories.Add(category);
        }
        else
        {
            var category = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

            if (category is null)
                return NotFound();

            category.Name = model.Name.Trim();
            category.Slug = await GenerateUniqueSlugAsync(model.Name, id);
            category.Color = model.Color?.Trim();
            category.Description = model.Description?.Trim();
            category.Order = model.Order;
            category.Status = model.Status;
        }

        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (category is null)
            return NotFound();

        category.DeletedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<string> GenerateUniqueSlugAsync(string name, string? excludeId = null)
    {
        var baseSlug = Slugify(name);
        var slug = baseSlug;
        var counter = 1;

        while (await _dbContext.Categories.AnyAsync(c =>
                   c.Slug == slug &&
                   c.DeletedAt == null &&
                   (excludeId == null || c.Id != excludeId)))
        {
            slug = $"{baseSlug}-{counter++}";
        }

        return slug;
    }

    private static string Slugify(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "danh-muc";

        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
        result = Regex.Replace(result, @"[\s-]+", "-").Trim('-');

        return string.IsNullOrEmpty(result) ? "danh-muc" : result;
    }
}
