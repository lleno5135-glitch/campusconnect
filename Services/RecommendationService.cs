using CampusConnect.Models;

namespace CampusConnect.Services;

public interface IRecommendationService
{
    IEnumerable<string> RecommendEvents(ApplicationUser? user, IEnumerable<CampusEvent> events);
    IEnumerable<ProjectTeam> RecommendTeams(ApplicationUser? user, IEnumerable<ProjectTeam> teams);
}

// Optional AI-ready recommendation layer. This local, explainable keyword matcher works
// without paid APIs or API keys; it can later be replaced by an AI provider.
public class RecommendationService : IRecommendationService
{
    public IEnumerable<string> RecommendEvents(ApplicationUser? user, IEnumerable<CampusEvent> events)
    {
        var interests = Words(user?.Interests);
        return events.Select(e => new { Event = e, Score = Words(e.Title + " " + e.Description).Intersect(interests).Count() })
            .OrderByDescending(x => x.Score).ThenBy(x => x.Event.StartsAt)
            .Select(x => x.Event.Title);
    }
    public IEnumerable<ProjectTeam> RecommendTeams(ApplicationUser? user, IEnumerable<ProjectTeam> teams)
    {
        var skills = Words(user?.Skills);
        return teams.Select(t => new { Team = t, Score = Words(t.RequiredSkills + " " + t.Description).Intersect(skills).Count() })
            .OrderByDescending(x => x.Score).ThenByDescending(x => x.Team.CreatedAt).Select(x => x.Team);
    }
    private static HashSet<string> Words(string? text) => (text ?? "")
        .Split(new[] { ',', ' ', ';', '/', '-', '.' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(x => x.Trim().ToLowerInvariant()).Where(x => x.Length > 2).ToHashSet();
}
