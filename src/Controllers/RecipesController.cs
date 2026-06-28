using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.Models;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Route("recipes")]
public class RecipesController : Controller
{
    private readonly AppDbContext _dbContext;

    public RecipesController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 10,
        string? q = null,
        List<string>? categoryIds = null,
        List<string>? difficulties = null,
        int maxTime = RecipeIndexViewModel.MaxCookingTimeLimit,
        RecipeSortBy sortBy = RecipeSortBy.Newest)
    {
        if (!RecipeIndexViewModel.AllowedPageSizes.Contains(pageSize))
            pageSize = 10;

        if (page < 1)
            page = 1;

        if (maxTime < 10)
            maxTime = 10;

        if (maxTime > RecipeIndexViewModel.MaxCookingTimeLimit)
            maxTime = RecipeIndexViewModel.MaxCookingTimeLimit;

        var categories = await _dbContext.Categories
            .Where(c => c.DeletedAt == null && c.Status == (int)RecordStatus.Active)
            .OrderBy(c => c.Order)
            .ThenBy(c => c.Name)
            .ToListAsync();

        var validCategoryIds = categories.Select(c => c.Id).ToHashSet();
        var selectedCategoryIds = (categoryIds ?? [])
            .Where(id => validCategoryIds.Contains(id))
            .Distinct()
            .ToList();

        var selectedDifficulties = (difficulties ?? [])
            .Where(d => RecipeIndexViewModel.AllowedDifficulties.Contains(d))
            .Distinct()
            .ToList();

        var searchQuery = string.IsNullOrWhiteSpace(q) ? null : q.Trim();

        var recipesQuery = _dbContext.Recipes
            .Where(r => r.DeletedAt == null && r.Status == (int)RecordStatus.Active);

        if (searchQuery is not null)
            recipesQuery = recipesQuery.Where(r => EF.Functions.Like(r.Title, $"%{searchQuery}%"));

        if (selectedCategoryIds.Count > 0)
            recipesQuery = recipesQuery.Where(r => selectedCategoryIds.Contains(r.CategoryId));

        if (selectedDifficulties.Count > 0)
            recipesQuery = recipesQuery.Where(r => selectedDifficulties.Contains(r.Difficulty));

        if (maxTime < RecipeIndexViewModel.MaxCookingTimeLimit)
        {
            recipesQuery = recipesQuery.Where(r =>
                r.CookingTimeMinutes != null && r.CookingTimeMinutes <= maxTime);
        }

        recipesQuery = sortBy switch
        {
            RecipeSortBy.Popular => recipesQuery
                .OrderByDescending(r => r.IsFeatured)
                .ThenByDescending(r => r.CreatedAt),
            RecipeSortBy.Fastest => recipesQuery
                .OrderBy(r => r.CookingTimeMinutes ?? int.MaxValue)
                .ThenByDescending(r => r.CreatedAt),
            _ => recipesQuery.OrderByDescending(r => r.CreatedAt)
        };

        var totalItems = await recipesQuery.CountAsync();
        var totalPages = totalItems == 0 ? 1 : (int)Math.Ceiling(totalItems / (double)pageSize);

        if (page > totalPages)
            page = totalPages;

        var recipes = await recipesQuery
            .Include(r => r.Category)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return View(new RecipeIndexViewModel
        {
            Categories = categories,
            Recipes = recipes,
            SearchQuery = searchQuery,
            SelectedCategoryIds = selectedCategoryIds,
            SelectedDifficulties = selectedDifficulties,
            MaxTime = maxTime,
            SortBy = sortBy,
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems
        });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var recipe = await _dbContext.Recipes
            .Include(r => r.Category)
            .FirstOrDefaultAsync(r =>
                r.Slug == slug
                && r.DeletedAt == null
                && r.Status == (int)RecordStatus.Active);

        if (recipe is null)
            return NotFound();

        return View(RecipeDetailViewModel.FromEntity(recipe));
    }
}
