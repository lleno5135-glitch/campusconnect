using System.Diagnostics;
using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

public class HomeController(ApplicationDbContext db, UserManager<ApplicationUser> users,
    IRecommendationService recommendations) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await users.GetUserAsync(User);
        var now = DateTime.UtcNow;
        ViewBag.Posts = await db.Posts.OrderByDescending(x => x.CreatedAt).Take(5).ToListAsync();
        ViewBag.Events = await db.Events.Where(x => x.StartsAt >= now).OrderBy(x => x.StartsAt).Take(4).ToListAsync();
        ViewBag.Announcements = await db.Announcements.OrderByDescending(x => x.CreatedAt).Take(3).ToListAsync();
        ViewBag.ResourcesCount = await db.Resources.CountAsync();
        ViewBag.TeamCount = await db.Teams.CountAsync();
        ViewBag.RecommendedEvents = recommendations.RecommendEvents(user,
            await db.Events.Where(x => x.StartsAt >= now).ToListAsync()).Take(3).ToList();
        return View();
    }
    public IActionResult Privacy() => View();
    public IActionResult Error() => View();
}
