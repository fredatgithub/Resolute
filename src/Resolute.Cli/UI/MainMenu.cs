using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class MainMenu
{
    private readonly ResolutionManager _resolutionManager;
    private readonly ReminderService _reminderService;
    private readonly StatisticsService _statisticsService;

    public MainMenu(ResolutionManager resolutionManager, ReminderService reminderService, StatisticsService statisticsService)
    {
        _resolutionManager = resolutionManager;
        _reminderService = reminderService;
        _statisticsService = statisticsService;
    }

    public async Task RenderAsync()
    {
        while (true)
        {
            Console.Clear();
            RenderHeader();
            await RenderUpcomingRemindersAsync();

            var choice = RenderMainMenuOptions();

            if (!await HandleMenuChoiceAsync(choice))
            {
                break;
            }
        }
    }

    private void RenderHeader()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════════════╗");
        Console.WriteLine("║   🎯 New Year's Resolution Tracker 2026 🎯   ║");
        Console.WriteLine("╚════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    private async Task RenderUpcomingRemindersAsync()
    {
        var dueResolutions = await _reminderService.GetDueRemindersAsync();

        if (dueResolutions.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⏰ You have {dueResolutions.Count()} resolution(s) due for check-in!");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private string RenderMainMenuOptions()
    {
        Console.WriteLine("Main Menu:");
        Console.WriteLine("  1. View All Resolutions");
        Console.WriteLine("  2. Add New Resolution");
        Console.WriteLine("  3. Check In on Resolution");
        Console.WriteLine("  4. View Upcoming Reminders");
        Console.WriteLine("  5. Statistics & Reports");
        Console.WriteLine("  6. Exit");
        Console.WriteLine();
        Console.Write("Select an option: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private async Task<bool> HandleMenuChoiceAsync(string choice)
    {
        switch (choice)
        {
            case "1":
                await new ViewResolutionsScreen(_resolutionManager).RenderAsync();
                break;
            case "2":
                await new AddResolutionScreen(_resolutionManager).RenderAsync();
                break;
            case "3":
                await new CheckInScreen(_resolutionManager, _reminderService).RenderAsync();
                break;
            case "4":
                await new UpcomingRemindersScreen(_reminderService).RenderAsync();
                break;
            case "5":
                await new StatisticsScreen(_statisticsService).RenderAsync();
                break;
            case "6":
                Console.WriteLine("\n👋 Goodbye! Keep working on those resolutions!");
                return false;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n❌ Invalid option. Please try again.");
                Console.ResetColor();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
        }

        return true;
    }
}
