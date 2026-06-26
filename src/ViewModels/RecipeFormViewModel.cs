using System.ComponentModel.DataAnnotations;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class IngredientItemViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Quantity { get; set; } = string.Empty;
}

public class StepItemViewModel
{
    public int Order { get; set; } = 1;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class NutritionFormViewModel
{
    public int? Calories { get; set; }
    public int? Protein { get; set; }
    public int? Carbs { get; set; }
    public int? Fat { get; set; }
}

public class CategorySelectItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

public class RecipeFormViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn danh mục.")]
    [Display(Name = "Danh mục")]
    public string CategoryId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tiêu đề không được để trống.")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Ảnh thumbnail")]
    public string? ThumbnailUrl { get; set; }

    public IFormFile? ThumbnailFile { get; set; }

    [Display(Name = "Mô tả ngắn")]
    public string? Summary { get; set; }

    [Display(Name = "Nội dung")]
    public string? Content { get; set; }

    public string IngredientsJson { get; set; } = "[]";

    public string StepsJson { get; set; } = "[]";

    public NutritionFormViewModel Nutrition { get; set; } = new();

    [Display(Name = "Thời gian nấu (phút)")]
    public int? CookingTimeMinutes { get; set; }

    [Display(Name = "Khẩu phần")]
    public int? Servings { get; set; }

    [Display(Name = "Độ khó")]
    public string Difficulty { get; set; } = RecipeDifficulty.EASY.ToString();

    [Display(Name = "Công thức nổi bật")]
    public bool IsFeatured { get; set; }

    [Display(Name = "Trạng thái")]
    public int Status { get; set; } = (int)RecordStatus.Draft;

    public List<CategorySelectItem> Categories { get; set; } = [];

    public bool IsEdit => !string.IsNullOrEmpty(Id);
}
