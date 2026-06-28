using System.Text.Json;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class RecipeDetailViewModel
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public int? CookingTimeMinutes { get; set; }
    public int? Servings { get; set; }
    public string DifficultyLabel { get; set; } = string.Empty;
    public bool IsPopular { get; set; }
    public IReadOnlyList<string> Ingredients { get; set; } = [];
    public IReadOnlyList<RecipeStepViewModel> Steps { get; set; } = [];
    public RecipeNutritionViewModel? Nutrition { get; set; }
    public string? Content { get; set; }

    public static RecipeDetailViewModel FromEntity(Recipe recipe)
    {
        return new RecipeDetailViewModel
        {
            Slug = recipe.Slug,
            Title = recipe.Title,
            Summary = recipe.Summary ?? string.Empty,
            ThumbnailUrl = recipe.ThumbnailUrl,
            CategoryId = recipe.CategoryId,
            CategoryName = recipe.Category?.Name ?? "Món ăn",
            CookingTimeMinutes = recipe.CookingTimeMinutes,
            Servings = recipe.Servings,
            DifficultyLabel = GetDifficultyLabel(recipe.Difficulty),
            IsPopular = recipe.IsFeatured,
            Ingredients = ParseIngredients(recipe.Ingredients),
            Steps = ParseSteps(recipe.Steps),
            Nutrition = ParseNutrition(recipe.Nutrition),
            Content = recipe.Content
        };
    }

    private static string GetDifficultyLabel(string difficulty) => difficulty switch
    {
        nameof(RecipeDifficulty.EASY) => "Dễ",
        nameof(RecipeDifficulty.MEDIUM) => "Trung bình",
        nameof(RecipeDifficulty.HARD) => "Khó",
        _ => difficulty
    };

    private static IReadOnlyList<string> ParseIngredients(string json)
    {
        try
        {
            var items = JsonSerializer.Deserialize<List<IngredientItemViewModel>>(json, JsonOptions) ?? [];
            return items
                .Select(i =>
                {
                    var name = i.Name.Trim();
                    var quantity = i.Quantity.Trim();
                    if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(quantity))
                        return $"{quantity} {name}";
                    return !string.IsNullOrEmpty(name) ? name : quantity;
                })
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    private static IReadOnlyList<RecipeStepViewModel> ParseSteps(string json)
    {
        try
        {
            var items = JsonSerializer.Deserialize<List<StepItemViewModel>>(json, JsonOptions) ?? [];
            return items
                .Where(s => !string.IsNullOrWhiteSpace(s.Title) || !string.IsNullOrWhiteSpace(s.Description))
                .OrderBy(s => s.Order)
                .Select(s => new RecipeStepViewModel(
                    string.IsNullOrWhiteSpace(s.Title) ? $"Bước {s.Order}" : s.Title.Trim(),
                    s.Description.Trim()))
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    private static RecipeNutritionViewModel? ParseNutrition(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return null;

        try
        {
            var data = JsonSerializer.Deserialize<NutritionFormViewModel>(json, JsonOptions);
            if (data is null)
                return null;

            if (data.Calories is null && data.Protein is null && data.Carbs is null && data.Fat is null)
                return null;

            return new RecipeNutritionViewModel
            {
                Calories = data.Calories.HasValue ? $"{data.Calories} kcal" : "—",
                Protein = data.Protein.HasValue ? $"{data.Protein}g" : "—",
                Fat = data.Fat.HasValue ? $"{data.Fat}g" : "—",
                Carbs = data.Carbs.HasValue ? $"{data.Carbs}g" : "—"
            };
        }
        catch
        {
            return null;
        }
    }
}

public record RecipeStepViewModel(string Title, string Description);

public class RecipeNutritionViewModel
{
    public string Calories { get; set; } = string.Empty;
    public string Protein { get; set; } = string.Empty;
    public string Fat { get; set; } = string.Empty;
    public string Carbs { get; set; } = string.Empty;
}
