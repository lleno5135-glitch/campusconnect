using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

[Authorize]
public class LostFoundController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index(string? status)
    {
        var query = db.LostFoundItems.AsNoTracking().OrderByDescending(x => x.ReportedAt);
        if (!string.IsNullOrWhiteSpace(status)) query = (IOrderedQueryable<LostFoundItem>)query.Where(x => x.Status == status).OrderByDescending(x => x.ReportedAt);
        ViewBag.Status = status; return View(await query.ToListAsync());
    }
    public IActionResult Create() => View(new LostFoundInput());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(LostFoundInput input)
    {
        if (!new[] { "Lost", "Found" }.Contains(input.Status)) ModelState.AddModelError("Status", "Choose Lost or Found.");
        if (!ModelState.IsValid) return View(input);
        var user = await users.GetUserAsync(User);
        db.LostFoundItems.Add(new LostFoundItem { ItemName = input.ItemName, Status = input.Status, Location = input.Location,
            Details = input.Details, ContactInfo = input.ContactInfo, UserId = user!.Id, ReporterName = user.FullName ?? user.UserName! });
        await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
}
