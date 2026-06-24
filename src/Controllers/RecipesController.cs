using Microsoft.AspNetCore.Mvc;

namespace RecipeWebsite.Web.Controllers;

[Route("recipes")]
public class RecipesController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
