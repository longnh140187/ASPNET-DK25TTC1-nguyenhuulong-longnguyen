using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Configuration;
using RecipeWebsite.Web.Constants;
using RecipeWebsite.Web.Data;

var rootEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (File.Exists(rootEnvPath))
    Env.Load(rootEnvPath);
else
    Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var dbOptions = new DatabaseOptions
{
    Host = Environment.GetEnvironmentVariable(AppConstants.DbHost) ?? string.Empty,
    Port = int.TryParse(Environment.GetEnvironmentVariable(AppConstants.DbPort), out var port) ? port : 0,
    Name = Environment.GetEnvironmentVariable(AppConstants.DbName) ?? string.Empty,
    User = Environment.GetEnvironmentVariable(AppConstants.DbUser) ?? string.Empty,
    Password = Environment.GetEnvironmentVariable(AppConstants.DbPassword) ?? string.Empty
};

var connectionString = dbOptions.BuildConnectionString();

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.36-mysql")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/manage";
        options.AccessDeniedPath = "/errors/403";
        options.ExpireTimeSpan = TimeSpan.FromHours(12);
        options.SlidingExpiration = true;
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/errors/500");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
