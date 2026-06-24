namespace RecipeWebsite.Web.Models;

public class Recipe : BaseEntity
{
    public string CategoryId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string Ingredients { get; set; } = "[]";
    public string Steps { get; set; } = "[]";
    public string? Nutrition { get; set; }
    public int? CookingTimeMinutes { get; set; }
    public int? Servings { get; set; }
    public string Difficulty { get; set; } = "EASY";
    public bool IsFeatured { get; set; } = false;
    public int Status { get; set; } = 2;

    public Category Category { get; set; } = null!;
}
