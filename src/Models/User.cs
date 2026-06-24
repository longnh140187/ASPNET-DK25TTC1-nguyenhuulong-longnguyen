namespace RecipeWebsite.Web.Models;

public class User : BaseEntity
{
    public string? Name { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public int Role { get; set; } = 0;
    public int Status { get; set; } = 1;
}
