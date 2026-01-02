using System;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class UpcomingRemindersScreen
{
    private readonly ReminderService _reminderService;

    public UpcomingRemindersScreen(ReminderService reminderService)
    {
        _reminderService = reminderService;
    }

    public async Task RenderAsync()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║    ⏰ Upcoming Reminders           ║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var upcomingReminders = (await _reminderService.GetUpcomingRemindersAsync(7)).ToList();

        if (!upcomingReminders.Any())
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✨ No reminders in the next 7 days. You're all caught up!");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"You have {upcomingReminders.Count} upcoming reminder(s) in the next 7 days:\n");

            foreach (var (resolution, reminderDate) in upcomingReminders)
            {
                var daysUntil = (reminderDate.Date - DateTime.Now.Date).Days;
                var daysText = daysUntil == 0 ? "TODAY" :
                               daysUntil == 1 ? "Tomorrow" :
                               $"In {daysUntil} days";

                Console.ForegroundColor = daysUntil == 0 ? ConsoleColor.Red : ConsoleColor.Yellow;
                Console.WriteLine($"📅 {reminderDate:MM/dd/yyyy} ({daysText})");
                Console.ResetColor();
                Console.WriteLine($"   {resolution.Title}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"   Category: {resolution.Category}");
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
