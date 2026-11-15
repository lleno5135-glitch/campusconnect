using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers;

[Authorize]
public class ResourcesController(ApplicationDbContext db, UserManager<ApplicationUser> users, IWebHostEnvironment env) : Controller
{
    public async Task<IActionResult> Index(string? q)
    {
        var query = db.Resources.AsNoTracking().OrderByDescending(x => x.CreatedAt);
        if (!string.IsNullOrWhiteSpace(q)) query = (IOrderedQueryable<StudyResource>)query.Where(x => x.Title.Contains(q) || x.Subject.Contains(q)).OrderByDescending(x => x.CreatedAt);
        ViewBag.Query = q;
        return View(await query.ToListAsync());
    }
    public IActionResult Create() => View(new ResourceInput());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ResourceInput input)
    {
        if (!ModelState.IsValid) return View(input);
        if (input.File == null || input.File.Length == 0) { ModelState.AddModelError("File", "Please choose a file."); return View(input); }
        var ext = Path.GetExtension(input.File.FileName).ToLowerInvariant();
        if (!new[] { ".pdf", ".doc", ".docx", ".ppt", ".pptx" }.Contains(ext) || input.File.Length > 10 * 1024 * 1024)
        { ModelState.AddModelError("File", "Allowed: PDF/DOC/DOCX/PPT/PPTX, maximum 10 MB."); return View(input); }
        var folder = Path.Combine(env.WebRootPath, "uploads");
        Directory.CreateDirectory(folder);
        var stored = $"{Guid.NewGuid():N}{ext}";
        await using (var stream = System.IO.File.Create(Path.Combine(folder, stored)))
            await input.File.CopyToAsync(stream);
        var user = await users.GetUserAsync(User);
        db.Resources.Add(new StudyResource { Title = input.Title, Subject = input.Subject, Department = input.Department,
            Description = input.Description, FileName = Path.GetFileName(input.File.FileName), StoredFileName = stored,
            UserId = user!.Id, AuthorName = user.FullName ?? user.UserName! });
        await db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Download(int id)
    {
        var item = await db.Resources.FindAsync(id);
        if (item == null) return NotFound();
        var path = Path.Combine(env.WebRootPath, "uploads", item.StoredFileName);
        if (!System.IO.File.Exists(path)) return NotFound();
        return PhysicalFile(path, "application/octet-stream", item.FileName);
    }
}
