using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp.Models;

namespace ConsoleApp.Services;

public class StatisticsService
{
    private readonly ResolutionManager _resolutionManager;

    public StatisticsService(ResolutionManager resolutionManager)
    {
        _resolutionManager = resolutionManager;
    }

    public StatisticsReport GenerateReport()
    {
        var allResolutions = _resolutionManager.GetAllResolutions().ToList();
        var activeResolutions = _resolutionManager.GetActiveResolutions().ToList();
        var completedResolutions = _resolutionManager.GetCompletedResolutions().ToList();

        return new StatisticsReport
        {
            TotalResolutions = allResolutions.Count,
            ActiveResolutions = activeResolutions.Count,
            CompletedResolutions = completedResolutions.Count,
            CompletionRate = allResolutions.Count > 0
                ? (double)completedResolutions.Count / allResolutions.Count * 100
                : 0,
            CategoryBreakdown = GetCategoryBreakdown(allResolutions),
            CurrentStreak = CalculateCurrentStreak(allResolutions),
            LongestStreak = CalculateLongestStreak(allResolutions),
            TotalCheckIns = allResolutions.Sum(r => r.CheckIns.Count),
            AverageCheckInsPerResolution = allResolutions.Count > 0
                ? (double)allResolutions.Sum(r => r.CheckIns.Count) / allResolutions.Count
                : 0,
            MostActiveCategory = GetMostActiveCategory(allResolutions),
            RecentActivity = GetRecentActivity(allResolutions)
        };
    }

    private Dictionary<string, CategoryStats> GetCategoryBreakdown(List<Resolution> resolutions)
    {
        return resolutions
            .GroupBy(r => r.Category)
            .ToDictionary(
                g => g.Key,
                g => new CategoryStats
                {
                    Total = g.Count(),
                    Active = g.Count(r => !r.IsCompleted),
                    Completed = g.Count(r => r.IsCompleted),
                    CompletionRate = g.Count() > 0
                        ? (double)g.Count(r => r.IsCompleted) / g.Count() * 100
                        : 0
                }
            );
    }

    private int CalculateCurrentStreak(List<Resolution> resolutions)
    {
        var allCheckIns = resolutions
            .SelectMany(r => r.CheckIns)
            .OrderByDescending(c => c.Date)
            .ToList();

        if (!allCheckIns.Any())
            return 0;

        int streak = 0;
        var currentDate = DateTime.Now.Date;

        foreach (var checkIn in allCheckIns)
        {
            var daysDiff = (currentDate - checkIn.Date.Date).Days;

            if (daysDiff <= 1 + streak)
            {
                if (checkIn.Date.Date != currentDate.AddDays(-streak))
                    continue;
                streak++;
                currentDate = checkIn.Date.Date;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    private int CalculateLongestStreak(List<Resolution> resolutions)
    {
        var allCheckIns = resolutions
            .SelectMany(r => r.CheckIns)
            .Select(c => c.Date.Date)
            .Distinct()
            .OrderBy(d => d)
            .ToList();

        if (!allCheckIns.Any())
            return 0;

        int longestStreak = 1;
        int currentStreak = 1;

        for (int i = 1; i < allCheckIns.Count; i++)
        {
            var daysDiff = (allCheckIns[i] - allCheckIns[i - 1]).Days;

            if (daysDiff == 1)
            {
                currentStreak++;
                longestStreak = Math.Max(longestStreak, currentStreak);
            }
            else
            {
                currentStreak = 1;
            }
        }

        return longestStreak;
    }

    private string GetMostActiveCategory(List<Resolution> resolutions)
    {
        if (!resolutions.Any())
            return "None";

        return resolutions
            .GroupBy(r => r.Category)
            .OrderByDescending(g => g.Sum(r => r.CheckIns.Count))
            .First()
            .Key;
    }

    private List<RecentActivityItem> GetRecentActivity(List<Resolution> resolutions)
    {
        var recentCheckIns = resolutions
            .SelectMany(r => r.CheckIns.Select(c => new { Resolution = r, CheckIn = c }))
            .OrderByDescending(x => x.CheckIn.Date)
            .Take(10)
            .Select(x => new RecentActivityItem
            {
                Date = x.CheckIn.Date,
                ResolutionTitle = x.Resolution.Title,
                Status = x.CheckIn.Status,
                Notes = x.CheckIn.Notes
            })
            .ToList();

        var recentCompletions = resolutions
            .Where(r => r.IsCompleted && r.CompletedDate.HasValue)
            .OrderByDescending(r => r.CompletedDate)
            .Take(5)
            .Select(r => new RecentActivityItem
            {
                Date = r.CompletedDate!.Value,
                ResolutionTitle = r.Title,
                Status = CheckInStatus.Completed,
                Notes = "Resolution completed! 🎉"
            });

        return recentCheckIns
            .Concat(recentCompletions)
            .OrderByDescending(a => a.Date)
            .Take(10)
            .ToList();
    }
}

public class StatisticsReport
{
    public int TotalResolutions { get; set; }
    public int ActiveResolutions { get; set; }
    public int CompletedResolutions { get; set; }
    public double CompletionRate { get; set; }
    public Dictionary<string, CategoryStats> CategoryBreakdown { get; set; } = new();
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public int TotalCheckIns { get; set; }
    public double AverageCheckInsPerResolution { get; set; }
    public string MostActiveCategory { get; set; } = string.Empty;
    public List<RecentActivityItem> RecentActivity { get; set; } = new();
}

public class CategoryStats
{
    public int Total { get; set; }
    public int Active { get; set; }
    public int Completed { get; set; }
    public double CompletionRate { get; set; }
}

public class RecentActivityItem
{
    public DateTime Date { get; set; }
    public string ResolutionTitle { get; set; } = string.Empty;
    public CheckInStatus Status { get; set; }
    public string Notes { get; set; } = string.Empty;
}
