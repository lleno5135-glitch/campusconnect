using CampusConnect.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<CampusPost> Posts => Set<CampusPost>();
    public DbSet<StudyResource> Resources => Set<StudyResource>();
    public DbSet<CampusEvent> Events => Set<CampusEvent>();
    public DbSet<ProjectTeam> Teams => Set<ProjectTeam>();
    public DbSet<LostFoundItem> LostFoundItems => Set<LostFoundItem>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CampusPost>().HasIndex(x => x.CreatedAt);
        builder.Entity<StudyResource>().HasIndex(x => x.CreatedAt);
        builder.Entity<EventRegistration>().HasIndex(x => new { x.EventId, x.UserId }).IsUnique();
    }
}
