namespace RecipeWebsite.Web.Configuration;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Name { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string BuildConnectionString()
    {
        return $"Server={Host};Port={Port};Database={Name};User={User};Password={Password};CharSet=utf8mb4;";
    }
}
