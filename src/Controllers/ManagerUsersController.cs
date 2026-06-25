using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.Models;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Authorize(Roles = "Admin")]
[Route("manager/users")]
public class ManagerUsersController : Controller
{
    private readonly AppDbContext _dbContext;

    public ManagerUsersController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        ViewData["ActiveMenu"] = "members";
        ViewData["Title"] = "Quản lý thành viên";
        ViewData["HeaderTitle"] = "Thành viên";
        ViewData["HeaderSubtitle"] = "Quản lý tài khoản người dùng";

        var users = await _dbContext.Users
            .Where(u => u.DeletedAt == null)
            .OrderBy(u => u.Username)
            .ToListAsync();

        return View(users);
    }

    [HttpGet("action")]
    public async Task<IActionResult> Action(string? id)
    {
        ViewData["ActiveMenu"] = "members";

        if (string.IsNullOrEmpty(id))
        {
            ViewData["Title"] = "Thêm thành viên";
            ViewData["HeaderTitle"] = "Thêm thành viên";
            ViewData["HeaderSubtitle"] = "Tạo tài khoản người dùng mới";
            return View(new UserFormViewModel());
        }

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

        if (user is null)
            return NotFound();

        ViewData["Title"] = "Sửa thành viên";
        ViewData["HeaderTitle"] = "Sửa thành viên";
        ViewData["HeaderSubtitle"] = "Cập nhật thông tin tài khoản";

        return View(new UserFormViewModel
        {
            Id = user.Id,
            Name = user.Name,
            Username = user.Username,
            Role = user.Role,
            Status = user.Status
        });
    }

    [HttpPost("action")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Action(UserFormViewModel model, string? id)
    {
        ViewData["ActiveMenu"] = "members";
        var isEdit = !string.IsNullOrEmpty(id);
        ViewData["Title"] = isEdit ? "Sửa thành viên" : "Thêm thành viên";
        ViewData["HeaderTitle"] = ViewData["Title"];
        ViewData["HeaderSubtitle"] = isEdit
            ? "Cập nhật thông tin tài khoản"
            : "Tạo tài khoản người dùng mới";

        if (!isEdit && string.IsNullOrWhiteSpace(model.Password))
            ModelState.AddModelError(nameof(model.Password), "Mật khẩu không được để trống.");

        var normalizedUsername = model.Username.Trim();
        var usernameExists = await _dbContext.Users.AnyAsync(u =>
            u.Username == normalizedUsername &&
            u.DeletedAt == null &&
            (!isEdit || u.Id != id));

        if (usernameExists)
            ModelState.AddModelError(nameof(model.Username), "Tên đăng nhập đã tồn tại.");

        if (!ModelState.IsValid)
            return View(model);

        if (!isEdit)
        {
            var user = new User
            {
                Name = model.Name?.Trim(),
                Username = normalizedUsername,
                PasswordHash = ComputeSha256(model.Password!),
                Role = model.Role,
                Status = model.Status
            };
            _dbContext.Users.Add(user);
        }
        else
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

            if (user is null)
                return NotFound();

            user.Name = model.Name?.Trim();
            user.Username = normalizedUsername;
            user.Role = model.Role;
            user.Status = model.Status;

            if (!string.IsNullOrWhiteSpace(model.Password))
                user.PasswordHash = ComputeSha256(model.Password);
        }

        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.DeletedAt == null);

        if (user is null)
            return NotFound();

        user.DeletedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private static string ComputeSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
