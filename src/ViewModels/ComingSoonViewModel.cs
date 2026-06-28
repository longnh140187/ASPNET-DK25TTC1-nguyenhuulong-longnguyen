namespace RecipeWebsite.Web.ViewModels;

public class ComingSoonViewModel
{
    public string? FeatureName { get; set; }

    public string Heading => string.IsNullOrWhiteSpace(FeatureName)
        ? "Sắp ra mắt"
        : $"{FeatureName.Trim()} — Sắp ra mắt";
}
