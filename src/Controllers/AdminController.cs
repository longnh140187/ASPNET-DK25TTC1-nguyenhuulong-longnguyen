using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _dbContext;

    public AdminController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalBlogs = await _dbContext.Blogs.CountAsync(b => b.DeletedAt == null),
            TotalRecipes = await _dbContext.Recipes.CountAsync(r => r.DeletedAt == null),
            TotalMembers = await _dbContext.Users.CountAsync(u => u.DeletedAt == null)
        };

        return View(model);
    }
}
