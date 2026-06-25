using System.ComponentModel.DataAnnotations;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class UserFormViewModel
{
    public string? Id { get; set; }

    [MaxLength(255)]
    [Display(Name = "Họ tên")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
    [MaxLength(255)]
    [Display(Name = "Tên đăng nhập")]
    public string Username { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string? Password { get; set; }

    [Display(Name = "Vai trò")]
    public int Role { get; set; } = (int)UserRole.User;

    [Display(Name = "Trạng thái")]
    public int Status { get; set; } = (int)RecordStatus.Active;

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}
