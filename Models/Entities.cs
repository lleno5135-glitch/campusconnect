using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Models;

public class ApplicationUser : IdentityUser
{
    [StringLength(80)] public string? FullName { get; set; }
    [StringLength(80)] public string? Department { get; set; }
    [StringLength(30)] public string? YearOfStudy { get; set; }
    [StringLength(500)] public string? Skills { get; set; }
    [StringLength(500)] public string? Interests { get; set; }
    [StringLength(500)] public string? Bio { get; set; }
}

public class CampusPost
{
    public int Id { get; set; }
    [Required, StringLength(160)] public string Title { get; set; } = "";
    [Required, StringLength(3000)] public string Body { get; set; } = "";
    [StringLength(40)] public string Category { get; set; } = "Discussion";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = "";
    public string AuthorName { get; set; } = "Student";
    public int Likes { get; set; }
}

public class StudyResource
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(100)] public string Subject { get; set; } = "";
    [StringLength(100)] public string Department { get; set; } = "";
    [StringLength(1000)] public string Description { get; set; } = "";
    public string FileName { get; set; } = "";
    public string StoredFileName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = "";
    public string AuthorName { get; set; } = "Student";
}

public class CampusEvent
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(2000)] public string Description { get; set; } = "";
    [StringLength(120)] public string Venue { get; set; } = "";
    public DateTime StartsAt { get; set; }
    public string CreatedById { get; set; } = "";
    public string CreatedByName { get; set; } = "";
}

public class EventRegistration
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string UserId { get; set; } = "";
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}

public class ProjectTeam
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string ProjectTitle { get; set; } = "";
    [StringLength(1000)] public string Description { get; set; } = "";
    [StringLength(300)] public string RequiredSkills { get; set; } = "";
    [StringLength(80)] public string Department { get; set; } = "";
    public int MembersNeeded { get; set; } = 2;
    public string OwnerId { get; set; } = "";
    public string OwnerName { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class LostFoundItem
{
    public int Id { get; set; }
    [Required, StringLength(120)] public string ItemName { get; set; } = "";
    [StringLength(30)] public string Status { get; set; } = "Lost";
    [StringLength(100)] public string Location { get; set; } = "";
    [StringLength(1000)] public string Details { get; set; } = "";
    [StringLength(120)] public string ContactInfo { get; set; } = "";
    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = "";
    public string ReporterName { get; set; } = "";
}

public class Announcement
{
    public int Id { get; set; }
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(2000)] public string Message { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string AuthorName { get; set; } = "Campus Admin";
}
