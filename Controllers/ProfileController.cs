using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Controllers;

[Authorize]
public class ProfileController(UserManager<ApplicationUser> users) : Controller
{
    public async Task<IActionResult> Index()
    {
        var u = await users.GetUserAsync(User);
        return View(new ProfileInput { FullName = u!.FullName ?? "", Department = u.Department ?? "", YearOfStudy = u.YearOfStudy ?? "", Skills = u.Skills ?? "", Interests = u.Interests ?? "", Bio = u.Bio ?? "" });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileInput input)
    {
        if (!ModelState.IsValid) return View(input);
        var u = await users.GetUserAsync(User);
        u!.FullName = input.FullName; u.Department = input.Department; u.YearOfStudy = input.YearOfStudy;
        u.Skills = input.Skills; u.Interests = input.Interests; u.Bio = input.Bio;
        await users.UpdateAsync(u); TempData["Notice"] = "Profile updated."; return RedirectToAction(nameof(Index));
    }
}
