namespace RecipeWebsite.Web.Constants;

public static class AppConstants
{
    public const string DbHost = "DB_HOST";
    public const string DbPort = "DB_PORT";
    public const string DbName = "DB_NAME";
    public const string DbUser = "DB_USER";
    public const string DbPassword = "DB_PASSWORD";

    public const string UploadFolder = "uploads/recipes";
    public const string UploadBlogFolder = "uploads/blogs";
    public const long MaxUploadBytes = 5 * 1024 * 1024;
}
