using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Models;

namespace RecipeWebsite.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Recipe> Recipes => Set<Recipe>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCategory(modelBuilder);
        ConfigureUser(modelBuilder);
        ConfigureRecipe(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyUtcTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyUtcTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyUtcTimestamps()
    {
        var utcNow = DateTime.UtcNow;
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                if (string.IsNullOrWhiteSpace(entry.Entity.Id))
                    entry.Entity.Id = Guid.NewGuid().ToString();

                entry.Entity.CreatedAt = utcNow;
            }

            entry.Entity.UpdatedAt = utcNow;
        }
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(36);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Slug).HasColumnName("slug").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Color).HasColumnName("color").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("text");
            entity.Property(e => e.Order).HasColumnName("order").HasDefaultValue(0);
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("datetime(3)");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime(3)");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("datetime(3)");

            entity.HasIndex(e => e.Slug).IsUnique();
        });
    }

    private static void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(36);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
            entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(255).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Role).HasColumnName("role").HasDefaultValue(0);
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValue(1);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("datetime(3)");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime(3)");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("datetime(3)");

            entity.HasIndex(e => e.Username).IsUnique();
        });
    }

    private static void ConfigureRecipe(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.ToTable("recipes");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(36);
            entity.Property(e => e.CategoryId).HasColumnName("category_id").HasMaxLength(36);
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Slug).HasColumnName("slug").HasMaxLength(300).IsRequired();
            entity.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url").HasMaxLength(500);
            entity.Property(e => e.Summary).HasColumnName("summary").HasColumnType("text");
            entity.Property(e => e.Content).HasColumnName("content").HasColumnType("longtext");
            entity.Property(e => e.Ingredients).HasColumnName("ingredients").HasColumnType("longtext").IsRequired();
            entity.Property(e => e.Steps).HasColumnName("steps").HasColumnType("longtext").IsRequired();
            entity.Property(e => e.Nutrition).HasColumnName("nutrition").HasColumnType("longtext");
            entity.Property(e => e.CookingTimeMinutes).HasColumnName("cooking_time_minutes");
            entity.Property(e => e.Servings).HasColumnName("servings");
            entity.Property(e => e.Difficulty).HasColumnName("difficulty").HasMaxLength(10).HasDefaultValue("EASY");
            entity.Property(e => e.IsFeatured).HasColumnName("is_featured").HasDefaultValue(false);
            entity.Property(e => e.Status).HasColumnName("status").HasDefaultValue(2);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasColumnType("datetime(3)");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasColumnType("datetime(3)");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at").HasColumnType("datetime(3)");

            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.CategoryId);

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
