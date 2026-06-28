using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class BlogDetailViewModel
{
    public Blog Blog { get; set; } = null!;
    public string TagLabel { get; set; } = "Bài viết";
    public string TagClass { get; set; } = "bg-primary-container text-white";
    public List<Blog> PopularBlogs { get; set; } = [];
    public List<Blog> RelatedBlogs { get; set; } = [];

    public static IEnumerable<string> SplitParagraphs(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return [];

        var parts = content
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(p => !string.IsNullOrWhiteSpace(p))
            .ToList();

        if (parts.Count > 0)
            return parts;

        return [content.Trim()];
    }
}
