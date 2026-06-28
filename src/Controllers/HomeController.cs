using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.Models;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var activeRecipesQuery = _dbContext.Recipes
            .Where(r => r.DeletedAt == null && r.Status == (int)RecordStatus.Active);

        var featuredRecipes = await activeRecipesQuery
            .Include(r => r.Category)
            .Where(r => r.IsFeatured)
            .OrderByDescending(r => r.CreatedAt)
            .Take(6)
            .ToListAsync();

        var randomRecipeIds = await activeRecipesQuery
            .Select(r => r.Id)
            .ToListAsync();

        var pickedIds = randomRecipeIds
            .OrderBy(_ => Random.Shared.Next())
            .Take(3)
            .ToList();

        var randomPopularRecipes = pickedIds.Count == 0
            ? []
            : await _dbContext.Recipes
                .Where(r => pickedIds.Contains(r.Id))
                .ToListAsync();

        randomPopularRecipes = pickedIds
            .Select(id => randomPopularRecipes.First(r => r.Id == id))
            .ToList();

        var latestBlogs = await _dbContext.Blogs
            .Where(b => b.DeletedAt == null && b.Status == BlogStatus.PUBLISHED)
            .OrderByDescending(b => b.CreatedAt)
            .Take(3)
            .ToListAsync();

        return View(new HomeIndexViewModel
        {
            FeaturedRecipes = featuredRecipes,
            RandomPopularRecipes = randomPopularRecipes,
            LatestBlogs = latestBlogs
        });
    }
}
