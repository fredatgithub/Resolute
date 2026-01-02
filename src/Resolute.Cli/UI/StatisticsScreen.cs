using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class StatisticsScreen
{
    private readonly StatisticsService _statisticsService;

    public StatisticsScreen(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task RenderAsync()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("╔════════════════════════════════════════════════╗");
        Console.WriteLine("║         📊 Statistics & Reports 📊            ║");
        Console.WriteLine("╚════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var report = _statisticsService.GenerateReport();

        DisplayOverview(report);
        DisplayCategoryBreakdown(report);
        DisplayStreakInfo(report);
        DisplayRecentActivity(report);

        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ReadKey();
    }

    private void DisplayOverview(StatisticsReport report)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("📈 Overview");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));

        Console.WriteLine($"Total Resolutions:     {report.TotalResolutions}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Active:                {report.ActiveResolutions}");
        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"Completed:             {report.CompletedResolutions}");
        Console.ResetColor();
        Console.WriteLine();

        var completionColor = report.CompletionRate >= 75 ? ConsoleColor.Green :
                             report.CompletionRate >= 50 ? ConsoleColor.Yellow :
                             report.CompletionRate >= 25 ? ConsoleColor.DarkYellow :
                             ConsoleColor.Red;

        Console.ForegroundColor = completionColor;
        Console.WriteLine($"Completion Rate:       {report.CompletionRate:F1}%");
        Console.ResetColor();

        Console.WriteLine($"Total Check-ins:       {report.TotalCheckIns}");
        Console.WriteLine($"Avg Check-ins/Goal:    {report.AverageCheckInsPerResolution:F1}");
        Console.WriteLine($"Most Active Category:  {report.MostActiveCategory}");
        Console.WriteLine();
    }

    private void DisplayCategoryBreakdown(StatisticsReport report)
    {
        if (!report.CategoryBreakdown.Any())
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("📂 Category Breakdown");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));

        foreach (var category in report.CategoryBreakdown.OrderByDescending(c => c.Value.Total))
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n{category.Key}:");
            Console.ResetColor();

            Console.WriteLine($"  Total: {category.Value.Total} | " +
                            $"Active: {category.Value.Active} | " +
                            $"Completed: {category.Value.Completed}");

            Console.Write("  Completion: ");
            Console.ForegroundColor = category.Value.CompletionRate >= 50 ? ConsoleColor.Green : ConsoleColor.Yellow;
            Console.Write($"{category.Value.CompletionRate:F1}%");
            Console.ResetColor();

            // Progress bar
            var barLength = 30;
            var filledLength = (int)(category.Value.CompletionRate / 100.0 * barLength);
            Console.Write(" [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('█', filledLength));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('░', barLength - filledLength));
            Console.ResetColor();
            Console.WriteLine("]");
        }

        Console.WriteLine();
    }

    private void DisplayStreakInfo(StatisticsReport report)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("🔥 Streak Information");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));

        Console.ForegroundColor = report.CurrentStreak > 0 ? ConsoleColor.Yellow : ConsoleColor.Gray;
        Console.WriteLine($"Current Streak:        {report.CurrentStreak} day{(report.CurrentStreak != 1 ? "s" : "")}");
        Console.ResetColor();

        Console.ForegroundColor = report.LongestStreak >= 7 ? ConsoleColor.Green : ConsoleColor.White;
        Console.WriteLine($"Longest Streak:        {report.LongestStreak} day{(report.LongestStreak != 1 ? "s" : "")}");
        Console.ResetColor();

        if (report.CurrentStreak > 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n🔥 Keep it up! Don't break the streak!");
            Console.ResetColor();
        }
        else if (report.TotalCheckIns > 0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n💪 Start a new streak by checking in today!");
            Console.ResetColor();
        }

        Console.WriteLine();
    }

    private void DisplayRecentActivity(StatisticsReport report)
    {
        if (!report.RecentActivity.Any())
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("📅 Recent Activity (Last 10)");
        Console.ResetColor();
        Console.WriteLine(new string('─', 50));

        foreach (var activity in report.RecentActivity)
        {
            var statusColor = activity.Status switch
            {
                CheckInStatus.Completed => ConsoleColor.Green,
                CheckInStatus.OnTrack => ConsoleColor.Cyan,
                CheckInStatus.Struggling => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };

            var daysAgo = (DateTime.Now - activity.Date).Days;
            var timeText = daysAgo == 0 ? "Today" :
                          daysAgo == 1 ? "Yesterday" :
                          $"{daysAgo} days ago";

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{activity.Date:MM/dd/yyyy} ({timeText}): ");
            Console.ResetColor();

            Console.ForegroundColor = statusColor;
            Console.Write($"{activity.Status}");
            Console.ResetColor();

            Console.Write($" - {activity.ResolutionTitle}");

            if (!string.IsNullOrWhiteSpace(activity.Notes))
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                var shortNotes = activity.Notes.Length > 40
                    ? activity.Notes.Substring(0, 37) + "..."
                    : activity.Notes;
                Console.Write($" | {shortNotes}");
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}
