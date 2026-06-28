using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace RecipeWebsite.Web.Services;

public static class SlugHelper
{
    public static string FromTitle(string text, string fallback = "item")
    {
        if (string.IsNullOrWhiteSpace(text))
            return fallback;

        text = text.Replace("đ", "d", StringComparison.Ordinal)
            .Replace("Đ", "d", StringComparison.Ordinal);

        var normalized = text.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        var result = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        result = Regex.Replace(result, @"[^a-z0-9\s-]", "");
        result = Regex.Replace(result, @"[\s-]+", "-").Trim('-');

        return string.IsNullOrEmpty(result) ? fallback : result;
    }
}
