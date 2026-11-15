using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Models;

public class PostInput
{
    [Required, StringLength(160)] public string Title { get; set; } = "";
    [Required, StringLength(3000)] public string Body { get; set; } = "";
    [StringLength(40)] public string Category { get; set; } = "Discussion";
}
public class ResourceInput
{
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(100)] public string Subject { get; set; } = "";
    [StringLength(100)] public string Department { get; set; } = "";
    [StringLength(1000)] public string Description { get; set; } = "";
    public IFormFile? File { get; set; }
}
public class EventInput
{
    [Required, StringLength(150)] public string Title { get; set; } = "";
    [StringLength(2000)] public string Description { get; set; } = "";
    [StringLength(120)] public string Venue { get; set; } = "";
    public DateTime StartsAt { get; set; } = DateTime.Now.AddDays(7);
}
public class TeamInput
{
    [Required, StringLength(150)] public string ProjectTitle { get; set; } = "";
    [StringLength(1000)] public string Description { get; set; } = "";
    [StringLength(300)] public string RequiredSkills { get; set; } = "";
    [StringLength(80)] public string Department { get; set; } = "";
    [Range(1, 20)] public int MembersNeeded { get; set; } = 2;
}
public class LostFoundInput
{
    [Required, StringLength(120)] public string ItemName { get; set; } = "";
    public string Status { get; set; } = "Lost";
    [StringLength(100)] public string Location { get; set; } = "";
    [StringLength(1000)] public string Details { get; set; } = "";
    [StringLength(120)] public string ContactInfo { get; set; } = "";
}
public class ProfileInput
{
    [StringLength(80)] public string FullName { get; set; } = "";
    [StringLength(80)] public string Department { get; set; } = "";
    [StringLength(30)] public string YearOfStudy { get; set; } = "";
    [StringLength(500)] public string Skills { get; set; } = "";
    [StringLength(500)] public string Interests { get; set; } = "";
    [StringLength(500)] public string Bio { get; set; } = "";
}
