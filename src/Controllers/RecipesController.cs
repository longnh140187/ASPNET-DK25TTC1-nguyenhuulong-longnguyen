using Microsoft.AspNetCore.Mvc;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Route("recipes")]
public class RecipesController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("{slug}")]
    public IActionResult Detail(string slug)
    {
        // TODO: load from database by slug
        var model = RecipeDetailViewModel.CreateSample(slug);
        return View(model);
    }
}
