using System.Globalization;
using System.Text;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class BlogIndexViewModel
{
    public static readonly int[] AllowedPageSizes = [3, 6, 9, 12];

    public List<Blog> Blogs { get; set; } = [];
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 3;
    public int TotalItems { get; set; }

    public int TotalPages => TotalItems == 0 ? 1 : (int)Math.Ceiling(TotalItems / (double)PageSize);

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public string BuildQueryString(int page)
    {
        var sb = new StringBuilder();
        sb.Append("?page=").Append(page);
        sb.Append("&pageSize=").Append(PageSize);
        return sb.ToString();
    }

    public static string FormatDate(DateTime date) =>
        date.ToString("dd 'Tháng' MM, yyyy", CultureInfo.GetCultureInfo("vi-VN"));

    public static (string Label, string BadgeClass) GetTagForBlog(Blog blog, int index)
    {
        var tags = new[]
        {
            ("Mẹo nhà bếp", "bg-secondary text-white"),
            ("Dinh dưỡng", "bg-secondary text-white"),
            ("Cẩm nang đi chợ", "bg-tertiary text-white"),
            ("Văn hóa ẩm thực", "bg-primary text-white"),
            ("Mẹo nhà bếp", "bg-primary-container text-white")
        };

        var tag = tags[index % tags.Length];
        return tag;
    }
}
