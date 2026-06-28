using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[AllowAnonymous]
[Route("coming-soon")]
public class ComingSoonController : Controller
{
    [HttpGet("")]
    public IActionResult Index(string? feature = null)
    {
        return View(new ComingSoonViewModel { FeatureName = feature });
    }
}
