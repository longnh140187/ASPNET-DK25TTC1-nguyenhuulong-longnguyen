using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeWebsite.Web.Data;
using RecipeWebsite.Web.Models;
using RecipeWebsite.Web.ViewModels;

namespace RecipeWebsite.Web.Controllers;

[Route("blogs")]
public class BlogsController : Controller
{
    private readonly AppDbContext _dbContext;

    public BlogsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
    {
        if (!BlogIndexViewModel.AllowedPageSizes.Contains(pageSize))
            pageSize = 3;

        if (page < 1)
            page = 1;

        var blogsQuery = _dbContext.Blogs
            .Where(b => b.DeletedAt == null && b.Status == BlogStatus.PUBLISHED);

        var totalItems = await blogsQuery.CountAsync();
        var totalPages = totalItems == 0 ? 1 : (int)Math.Ceiling(totalItems / (double)pageSize);

        if (page > totalPages)
            page = totalPages;

        var blogs = await blogsQuery
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return View(new BlogIndexViewModel
        {
            Blogs = blogs,
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalItems
        });
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var blog = await _dbContext.Blogs
            .FirstOrDefaultAsync(b =>
                b.Slug == slug
                && b.DeletedAt == null
                && b.Status == BlogStatus.PUBLISHED);

        if (blog is null)
            return NotFound();

        var otherBlogs = await _dbContext.Blogs
            .Where(b =>
                b.DeletedAt == null
                && b.Status == BlogStatus.PUBLISHED
                && b.Id != blog.Id)
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .ToListAsync();

        var tagIndex = Math.Abs(StringComparer.Ordinal.GetHashCode(blog.Slug)) % 5;
        var (tagLabel, tagClass) = BlogIndexViewModel.GetTagForBlog(blog, tagIndex);

        return View(new BlogDetailViewModel
        {
            Blog = blog,
            TagLabel = tagLabel,
            TagClass = tagClass,
            PopularBlogs = otherBlogs.Take(3).ToList(),
            RelatedBlogs = otherBlogs.Take(3).ToList()
        });
    }
}
