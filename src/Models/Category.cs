namespace RecipeWebsite.Web.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Color { get; set; }
    public string? Description { get; set; }
    public int Order { get; set; } = 0;
    public int Status { get; set; } = 1;
}
