namespace RecipeWebsite.Web.Constants;

public static class AppConstants
{
    // Database env keys
    public const string DbHost = "DB_HOST";
    public const string DbPort = "DB_PORT";
    public const string DbName = "DB_NAME";
    public const string DbUser = "DB_USER";
    public const string DbPassword = "DB_PASSWORD";

    // App
    public const string AppName = "Nấu Ngon Mỗi Ngày";
    public const string AdminArea = "Admin";

    // Roles (for future Identity integration)
    public const string RoleAdmin = "Admin";
    public const string RoleUser = "User";

    // Upload (for future use)
    public const string UploadFolder = "uploads/recipes";
    public const long MaxUploadBytes = 5 * 1024 * 1024; // 5MB
}
