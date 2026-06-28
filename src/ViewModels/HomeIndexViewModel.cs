using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.ViewModels;

public class HomeIndexViewModel
{
    public List<Recipe> FeaturedRecipes { get; set; } = [];
    public List<Recipe> RandomPopularRecipes { get; set; } = [];
    public List<Blog> LatestBlogs { get; set; } = [];
}
