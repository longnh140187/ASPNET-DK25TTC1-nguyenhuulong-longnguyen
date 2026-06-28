using System.Text;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class RecipeIndexViewModel
{
    public const int MaxCookingTimeLimit = 120;

    public static readonly int[] AllowedPageSizes = [5, 10, 15, 20];

    public static readonly string[] AllowedDifficulties =
        [nameof(RecipeDifficulty.EASY), nameof(RecipeDifficulty.MEDIUM), nameof(RecipeDifficulty.HARD)];

    public List<Category> Categories { get; set; } = [];
    public List<Recipe> Recipes { get; set; } = [];
    public List<string> SelectedCategoryIds { get; set; } = [];
    public List<string> SelectedDifficulties { get; set; } = [];
    public string? SearchQuery { get; set; }
    public int MaxTime { get; set; } = MaxCookingTimeLimit;
    public RecipeSortBy SortBy { get; set; } = RecipeSortBy.Newest;
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }

    public int TotalPages => TotalItems == 0 ? 1 : (int)Math.Ceiling(TotalItems / (double)PageSize);

    public bool HasPreviousPage => CurrentPage > 1;

    public bool HasNextPage => CurrentPage < TotalPages;

    public bool HasActiveFilters =>
        !string.IsNullOrWhiteSpace(SearchQuery)
        || SelectedCategoryIds.Count > 0
        || SelectedDifficulties.Count > 0
        || MaxTime < MaxCookingTimeLimit
        || SortBy != RecipeSortBy.Newest;

    public string BuildQueryString(int page)
    {
        var sb = new StringBuilder();
        sb.Append("?page=").Append(page);
        sb.Append("&pageSize=").Append(PageSize);
        sb.Append("&sortBy=").Append(SortBy);

        if (!string.IsNullOrWhiteSpace(SearchQuery))
            sb.Append("&q=").Append(Uri.EscapeDataString(SearchQuery));

        if (MaxTime < MaxCookingTimeLimit)
            sb.Append("&maxTime=").Append(MaxTime);

        foreach (var id in SelectedCategoryIds)
            sb.Append("&categoryIds=").Append(Uri.EscapeDataString(id));

        foreach (var difficulty in SelectedDifficulties)
            sb.Append("&difficulties=").Append(Uri.EscapeDataString(difficulty));

        return sb.ToString();
    }
}
