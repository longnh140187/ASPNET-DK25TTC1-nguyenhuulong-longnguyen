using System.ComponentModel.DataAnnotations;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class BlogFormViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Ảnh thumbnail")]
    public string? ThumbnailUrl { get; set; }

    public IFormFile? ThumbnailFile { get; set; }

    [Display(Name = "Mô tả ngắn")]
    public string? Summary { get; set; }

    [Required(ErrorMessage = "Nội dung không được để trống.")]
    [Display(Name = "Nội dung")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Trạng thái")]
    public BlogStatus Status { get; set; } = BlogStatus.DRAFT;

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}
