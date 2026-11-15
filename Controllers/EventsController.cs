using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

[Authorize]
public class EventsController(ApplicationDbContext db, UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Events.OrderBy(x => x.StartsAt).ToListAsync());
    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View(new EventInput());
    [HttpPost, Authorize(Roles = "Admin"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventInput input)
    {
        if (!ModelState.IsValid) return View(input);
        db.Events.Add(new CampusEvent { Title = input.Title, Description = input.Description, Venue = input.Venue,
            StartsAt = input.StartsAt, CreatedById = users.GetUserId(User)!, CreatedByName = User.Identity!.Name! });
        await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int id)
    {
        var userId = users.GetUserId(User)!;
        if (await db.EventRegistrations.AnyAsync(x => x.EventId == id && x.UserId == userId))
            TempData["Notice"] = "You are already registered for this event.";
        else if (await db.Events.AnyAsync(x => x.Id == id))
        { db.EventRegistrations.Add(new EventRegistration { EventId = id, UserId = userId }); await db.SaveChangesAsync(); TempData["Notice"] = "Registration successful."; }
        return RedirectToAction(nameof(Index));
    }
}
