using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Users = await db.Users.CountAsync();
        ViewBag.Posts = await db.Posts.CountAsync();
        ViewBag.Resources = await db.Resources.CountAsync();
        ViewBag.Events = await db.Events.CountAsync();
        return View(await db.Announcements.OrderByDescending(x => x.CreatedAt).ToListAsync());
    }
    public IActionResult Announcement() => View(new Announcement());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Announcement(Announcement input)
    {
        if (!ModelState.IsValid) return View(input);
        input.AuthorName = User.Identity?.Name ?? "Campus Admin"; input.CreatedAt = DateTime.UtcNow;
        db.Announcements.Add(input); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAnnouncement(int id)
    {
        var item = await db.Announcements.FindAsync(id);
        if (item != null) { db.Announcements.Remove(item); await db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
