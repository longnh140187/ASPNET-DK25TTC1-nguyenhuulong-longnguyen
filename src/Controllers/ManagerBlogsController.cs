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
[Route("manager/blogs")]
public class ManagerBlogsController : Controller
{
    private static readonly string[] AllowedImageExtensions = [".png", ".jpg", ".jpeg", ".gif", ".webp", ".svg"];

    private readonly AppDbContext _dbContext;
    private readonly IWebHostEnvironment _environment;

    public ManagerBlogsController(AppDbContext dbContext, IWebHostEnvironment environment)
    {
        _dbContext = dbContext;
        _environment = environment;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "articles";
        ViewData["Title"] = "Quản lý bài viết";
        ViewData["HeaderTitle"] = "Bài viết";
        ViewData["HeaderSubtitle"] = "Quản lý danh sách bài viết blog";

        var blogs = await _dbContext.Blogs
            .Where(b => b.DeletedAt == null)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return View(blogs);
    }

    [HttpGet("action")]
    public async Task<IActionResult> Action(string? id, string? cloneId)
    {
        ViewData["ActiveMenu"] = "articles";

        if (!string.IsNullOrEmpty(cloneId))
        {
            var source = await _dbContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == cloneId && b.DeletedAt == null);

            if (source is null)
                return NotFound();

            var model = MapToFormViewModel(source);
            model.Id = null;
            model.Title = $"{source.Title} (bản sao)";

            ViewData["Title"] = "Nhân bản bài viết";
            ViewData["HeaderTitle"] = "Nhân bản bài viết";
            ViewData["HeaderSubtitle"] = "Chỉnh sửa thông tin và lưu thành bài viết mới";
            ViewData["IsClone"] = true;
            return View(model);
        }

        if (string.IsNullOrEmpty(id))
        {
            ViewData["Title"] = "Thêm bài viết";
            ViewData["HeaderTitle"] = "Thêm bài viết";
            ViewData["HeaderSubtitle"] = "Tạo bài viết mới";
            return View(new BlogFormViewModel());
        }

        var blog = await _dbContext.Blogs
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

        if (blog is null)
            return NotFound();

        ViewData["Title"] = "Sửa bài viết";
        ViewData["HeaderTitle"] = "Sửa bài viết";
        ViewData["HeaderSubtitle"] = "Cập nhật nội dung bài viết";

        return View(MapToFormViewModel(blog));
    }

    [HttpPost("action")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Action(BlogFormViewModel model, string? id)
    {
        ViewData["ActiveMenu"] = "articles";
        var isEdit = !string.IsNullOrEmpty(id);
        ViewData["Title"] = isEdit ? "Sửa bài viết" : "Thêm bài viết";
        ViewData["HeaderTitle"] = ViewData["Title"];
        ViewData["HeaderSubtitle"] = isEdit ? "Cập nhật nội dung bài viết" : "Tạo bài viết mới";

        ValidateThumbnailFile(model.ThumbnailFile);

        if (!ModelState.IsValid)
            return View(model);

        var thumbnailUrl = await SaveThumbnailAsync(model.ThumbnailFile, model.ThumbnailUrl);

        if (!isEdit)
        {
            var blog = new Blog
            {
                Title = model.Title.Trim(),
                Slug = await GenerateUniqueSlugAsync(model.Title),
                ThumbnailUrl = thumbnailUrl,
                Summary = model.Summary?.Trim(),
                Content = model.Content.Trim(),
                Status = model.Status
            };
            _dbContext.Blogs.Add(blog);
        }
        else
        {
            var blog = await _dbContext.Blogs
                .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

            if (blog is null)
                return NotFound();

            blog.Title = model.Title.Trim();
            blog.Slug = await GenerateUniqueSlugAsync(model.Title, id);
            blog.ThumbnailUrl = thumbnailUrl;
            blog.Summary = model.Summary?.Trim();
            blog.Content = model.Content.Trim();
            blog.Status = model.Status;
        }

        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var blog = await _dbContext.Blogs
            .FirstOrDefaultAsync(b => b.Id == id && b.DeletedAt == null);

        if (blog is null)
            return NotFound();

        blog.DeletedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private static BlogFormViewModel MapToFormViewModel(Blog blog) =>
        new()
        {
            Id = blog.Id,
            Title = blog.Title,
            ThumbnailUrl = blog.ThumbnailUrl,
            Summary = blog.Summary,
            Content = blog.Content,
            Status = blog.Status
        };

    private void ValidateThumbnailFile(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedImageExtensions.Contains(ext))
        {
            ModelState.AddModelError(nameof(BlogFormViewModel.ThumbnailFile),
                "Chỉ chấp nhận file ảnh: png, jpg, jpeg, gif, webp, svg.");
            return;
        }

        if (file.Length > AppConstants.MaxUploadBytes)
        {
            ModelState.AddModelError(nameof(BlogFormViewModel.ThumbnailFile),
                "Kích thước file tối đa 5MB.");
        }
    }

    private async Task<string?> SaveThumbnailAsync(IFormFile? file, string? existingUrl)
    {
        if (file is null || file.Length == 0)
            return existingUrl;

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var uploadDir = Path.Combine(_environment.WebRootPath, AppConstants.UploadBlogFolder);
        Directory.CreateDirectory(uploadDir);

        var fileName = $"{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(uploadDir, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return $"/{AppConstants.UploadBlogFolder}/{fileName}";
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, string? excludeId = null)
    {
        var baseSlug = Slugify(title);
        var slug = baseSlug;
        var counter = 1;

        while (await _dbContext.Blogs.AnyAsync(b =>
                   b.Slug == slug &&
                   b.DeletedAt == null &&
                   (excludeId == null || b.Id != excludeId)))
        {
            slug = $"{baseSlug}-{counter++}";
        }

        return slug;
    }

    private static string Slugify(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "bai-viet";

        var result = text.Trim().ToLowerInvariant();
        result = Regex.Replace(result, @"[\s]+", "-");
        result = Regex.Replace(result, @"[^\p{L}\p{N}-]", "");
        result = Regex.Replace(result, @"-+", "-").Trim('-');

        return string.IsNullOrEmpty(result) ? "bai-viet" : result;
    }
}
