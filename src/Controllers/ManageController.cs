using Microsoft.AspNetCore.Mvc;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Route("manage")]
public class ManageController : Controller
{
    [HttpGet("")]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // TODO: integrate ASP.NET Core Identity
        ModelState.AddModelError(string.Empty, "Chức năng đăng nhập sẽ được kích hoạt sau khi tích hợp Identity.");
        return View(model);
    }
}
