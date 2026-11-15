using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

[Authorize]
public class TeamsController(ApplicationDbContext db, UserManager<ApplicationUser> users, IRecommendationService recommender) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await users.GetUserAsync(User);
        var all = await db.Teams.AsNoTracking().ToListAsync();
        ViewBag.Recommended = recommender.RecommendTeams(user, all).Take(3).Select(x => x.Id).ToHashSet();
        return View(all.OrderByDescending(x => x.CreatedAt).ToList());
    }
    public IActionResult Create() => View(new TeamInput());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TeamInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var user = await users.GetUserAsync(User);
        db.Teams.Add(new ProjectTeam { ProjectTitle = input.ProjectTitle, Description = input.Description,
            RequiredSkills = input.RequiredSkills, Department = input.Department, MembersNeeded = input.MembersNeeded,
            OwnerId = user!.Id, OwnerName = user.FullName ?? user.UserName! });
        await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
    }
}
