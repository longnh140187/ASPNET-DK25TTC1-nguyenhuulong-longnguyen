namespace RecipeWebsite.Web.Models;

public class Blog : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Summary { get; set; }
    public string Content { get; set; } = string.Empty;
    public BlogStatus Status { get; set; } = BlogStatus.DRAFT;
}
