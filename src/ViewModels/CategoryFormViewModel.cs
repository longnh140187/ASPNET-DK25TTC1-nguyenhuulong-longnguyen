using System.ComponentModel.DataAnnotations;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class CategoryFormViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục không được để trống.")]
    [MaxLength(255, ErrorMessage = "Tên danh mục tối đa 255 ký tự.")]
    [Display(Name = "Tên danh mục")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    [Display(Name = "Màu sắc")]
    public string? Color { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Display(Name = "Thứ tự")]
    public int Order { get; set; }

    [Display(Name = "Trạng thái")]
    public int Status { get; set; } = (int)RecordStatus.Active;

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}
