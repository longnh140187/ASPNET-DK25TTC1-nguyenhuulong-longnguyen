using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Constants;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.Models;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("manager/recipes")]
public class ManagerRecipesController : Controller
{
    private static readonly string[] AllowedImageExtensions = [".png", ".jpg", ".jpeg", ".gif", ".webp", ".svg"];
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public ManagerRecipesController(AppDbContext dbContext, IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "recipes";
        ViewData["Title"] = "Quản lý công thức";
        ViewData["HeaderTitle"] = "Công thức";
        ViewData["HeaderSubtitle"] = "Quản lý danh sách công thức nấu ăn";

        var recipes = await _dbContext.Recipes
            .Include(r => r.Category)
            .Where(r => r.DeletedAt == null)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return View(recipes);
    }

    [HttpGet("action")]
    public async Task<IActionResult> Action(string? id, string? cloneId)
    {
        ViewData["ActiveMenu"] = "recipes";
        var categories = await GetCategoryOptionsAsync();

        if (!string.IsNullOrEmpty(cloneId))
        {
            var source = await _dbContext.Recipes
                .FirstOrDefaultAsync(r => r.Id == cloneId && r.DeletedAt == null);

            if (source is null)
                return NotFound();

            var model = MapToFormViewModel(source, categories);
            model.Id = null;
            model.Title = $"{source.Title} (bản sao)";

            ViewData["Title"] = "Nhân bản công thức";
            ViewData["HeaderTitle"] = "Nhân bản công thức";
            ViewData["HeaderSubtitle"] = "Chỉnh sửa thông tin và lưu thành công thức mới";
            ViewData["IsClone"] = true;
            return View(model);
        }

        if (string.IsNullOrEmpty(id))
        {
            ViewData["Title"] = "Thêm công thức";
            ViewData["HeaderTitle"] = "Thêm công thức";
            ViewData["HeaderSubtitle"] = "Tạo công thức mới";
            return View(new RecipeFormViewModel { Categories = categories });
        }

        var recipe = await _dbContext.Recipes
            .FirstOrDefaultAsync(r => r.Id == id && r.DeletedAt == null);

        if (recipe is null)
            return NotFound();

        ViewData["Title"] = "Sửa công thức";
        ViewData["HeaderTitle"] = "Sửa công thức";
        ViewData["HeaderSubtitle"] = "Cập nhật thông tin công thức";

        return View(MapToFormViewModel(recipe, categories));
    }

    [HttpPost("action")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Action(RecipeFormViewModel model, string? id)
    {
        ViewData["ActiveMenu"] = "recipes";
        var isEdit = !string.IsNullOrEmpty(id);
        ViewData["Title"] = isEdit ? "Sửa công thức" : "Thêm công thức";
        ViewData["HeaderTitle"] = ViewData["Title"];
        ViewData["HeaderSubtitle"] = isEdit ? "Cập nhật thông tin công thức" : "Tạo công thức mới";
        model.Categories = await GetCategoryOptionsAsync();

        ValidateJsonFields(model);
        ValidateThumbnailFile(model.ThumbnailFile);

        if (!ModelState.IsValid)
            return View(model);

        var thumbnailUrl = await SaveThumbnailAsync(model.ThumbnailFile, model.ThumbnailUrl);

        if (!isEdit)
        {
            var recipe = new Recipe
            {
                CategoryId = model.CategoryId,
                Title = model.Title.Trim(),
                Slug = await GenerateUniqueSlugAsync(model.Title),
                ThumbnailUrl = thumbnailUrl,
                Summary = model.Summary?.Trim(),
                Content = model.Content?.Trim(),
                Ingredients = model.IngredientsJson,
                Steps = model.StepsJson,
                Nutrition = SerializeNutrition(model.Nutrition),
                CookingTimeMinutes = model.CookingTimeMinutes,
                Servings = model.Servings,
                Difficulty = model.Difficulty,
                IsFeatured = model.IsFeatured,
                Status = model.Status
            };
            _dbContext.Recipes.Add(recipe);
        }
        else
        {
            var recipe = await _dbContext.Recipes
                .FirstOrDefaultAsync(r => r.Id == id && r.DeletedAt == null);

            if (recipe is null)
                return NotFound();

            recipe.CategoryId = model.CategoryId;
            recipe.Title = model.Title.Trim();
            recipe.Slug = await GenerateUniqueSlugAsync(model.Title, id);
            recipe.ThumbnailUrl = thumbnailUrl;
            recipe.Summary = model.Summary?.Trim();
            recipe.Content = model.Content?.Trim();
            recipe.Ingredients = model.IngredientsJson;
            recipe.Steps = model.StepsJson;
            recipe.Nutrition = SerializeNutrition(model.Nutrition);
            recipe.CookingTimeMinutes = model.CookingTimeMinutes;
            recipe.Servings = model.Servings;
            recipe.Difficulty = model.Difficulty;
            recipe.IsFeatured = model.IsFeatured;
            recipe.Status = model.Status;
        }

        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var recipe = await _dbContext.Recipes
            .FirstOrDefaultAsync(r => r.Id == id && r.DeletedAt == null);

        if (recipe is null)
            return NotFound();

        recipe.DeletedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<CategorySelectItem>> GetCategoryOptionsAsync() =>
        await _dbContext.Categories
            .Where(c => c.DeletedAt == null && c.Status == (int)RecordStatus.Active)
            .OrderBy(c => c.Order)
            .ThenBy(c => c.Name)
            .Select(c => new CategorySelectItem { Id = c.Id, Name = c.Name })
            .ToListAsync();

    private static RecipeFormViewModel MapToFormViewModel(Recipe recipe, List<CategorySelectItem> categories) =>
        new()
        {
            Id = recipe.Id,
            CategoryId = recipe.CategoryId,
            Title = recipe.Title,
            ThumbnailUrl = recipe.ThumbnailUrl,
            Summary = recipe.Summary,
            Content = recipe.Content,
            IngredientsJson = recipe.Ingredients,
            StepsJson = recipe.Steps,
            Nutrition = DeserializeNutrition(recipe.Nutrition),
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            Servings = recipe.Servings,
            Difficulty = recipe.Difficulty,
            IsFeatured = recipe.IsFeatured,
            Status = recipe.Status,
            Categories = categories
        };

    private void ValidateJsonFields(RecipeFormViewModel model)
    {
        if (!TryNormalizeIngredientsJson(model.IngredientsJson, out var ingredientsJson))
            ModelState.AddModelError(nameof(model.IngredientsJson), "Dữ liệu nguyên liệu không hợp lệ.");
        else
            model.IngredientsJson = ingredientsJson;

        if (!TryNormalizeStepsJson(model.StepsJson, out var stepsJson))
            ModelState.AddModelError(nameof(model.StepsJson), "Dữ liệu các bước nấu không hợp lệ.");
        else
            model.StepsJson = stepsJson;
    }

    private static bool TryNormalizeIngredientsJson(string json, out string normalized)
    {
        normalized = "[]";
        try
        {
            var items = JsonSerializer.Deserialize<List<IngredientItemViewModel>>(json, JsonOptions) ?? [];
            var cleaned = items
                .Where(i => !string.IsNullOrWhiteSpace(i.Name) || !string.IsNullOrWhiteSpace(i.Quantity))
                .Select(i => new IngredientItemViewModel
                {
                    Name = i.Name.Trim(),
                    Quantity = i.Quantity.Trim()
                })
                .ToList();
            normalized = JsonSerializer.Serialize(cleaned, JsonOptions);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool TryNormalizeStepsJson(string json, out string normalized)
    {
        normalized = "[]";
        try
        {
            var items = JsonSerializer.Deserialize<List<StepItemViewModel>>(json, JsonOptions) ?? [];
            var cleaned = items
                .Where(s => !string.IsNullOrWhiteSpace(s.Title) || !string.IsNullOrWhiteSpace(s.Description))
                .Select((s, index) => new StepItemViewModel
                {
                    Order = s.Order > 0 ? s.Order : index + 1,
                    Title = s.Title.Trim(),
                    Description = s.Description.Trim()
                })
                .ToList();
            normalized = JsonSerializer.Serialize(cleaned, JsonOptions);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static string? SerializeNutrition(NutritionFormViewModel nutrition)
    {
        if (nutrition.Calories is null && nutrition.Protein is null && nutrition.Carbs is null && nutrition.Fat is null)
            return null;

        return JsonSerializer.Serialize(nutrition, JsonOptions);
    }

    private static NutritionFormViewModel DeserializeNutrition(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new NutritionFormViewModel();

        try
        {
            return JsonSerializer.Deserialize<NutritionFormViewModel>(json, JsonOptions) ?? new NutritionFormViewModel();
        }
        catch
        {
            return new NutritionFormViewModel();
        }
    }

    private void ValidateThumbnailFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(ext))
        {
            ModelState.AddModelError(nameof(RecipeFormViewModel.ThumbnailFile),
                "Chỉ chấp nhận file ảnh: png, jpg, jpeg, gif, webp, svg.");
            return;
        }

        if (file.Length > AppConstants.MaxUploadBytes)
        {
            ModelState.AddModelError(nameof(RecipeFormViewModel.ThumbnailFile),
                "Kích thước file tối đa 5MB.");
        }
    }

    private async Task<string?> SaveThumbnailAsync(IFormFile? file, string? existingUrl)
    {
        if (file is null || file.Length == 0)
            return existingUrl;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var uploadDir = Path.Combine(_environment.WebRootPath, AppConstants.UploadFolder);
        Directory.CreateDirectory(uploadDir);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(uploadDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{AppConstants.UploadFolder}/{fileName}";
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, string? excludeId = null)
    {
        var baseSlug = Slugify(title);
        var slug = baseSlug;
        var counter = 1;

        while (await _dbContext.Recipes.AnyAsync(r =>
                   r.Slug == slug &&
                   r.DeletedAt == null &&
                   (excludeId == null || r.Id != excludeId)))
        {
            slug = $"{baseSlug}-{counter++}";
        }

        return slug;
    }

    private static string Slugify(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "cong-thuc";

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

        return string.IsNullOrEmpty(result) ? "cong-thuc" : result;
    }
}
