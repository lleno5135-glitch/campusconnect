using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

[Authorize]
public class CommunityController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(string? q)
    {
        var posts = db.Posts.AsNoTracking().OrderByDescending(x => x.CreatedAt);
        if (!string.IsNullOrWhiteSpace(q)) posts = (IOrderedQueryable<CampusPost>)posts.Where(x => x.Title.Contains(q) || x.Body.Contains(q)).OrderByDescending(x => x.CreatedAt);
        ViewBag.Query = q;
        return View(await posts.ToListAsync());
    }
    public IActionResult Create() => View(new PostInput());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PostInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var user = await users.GetUserAsync(User);
        db.Posts.Add(new CampusPost { Title = input.Title, Body = input.Body, Category = input.Category,
            UserId = user!.Id, AuthorName = user.FullName ?? user.UserName! });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Like(int id)
    {
        var post = await db.Posts.FindAsync(id);
        if (post != null) { post.Likes++; await db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await db.Posts.FindAsync(id);
        if (post != null && (post.UserId == users.GetUserId(User) || User.IsInRole("Admin")))
        { db.Posts.Remove(post); await db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
