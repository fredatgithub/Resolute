using System;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class AddResolutionScreen
{
    private readonly ResolutionManager _resolutionManager;

    public AddResolutionScreen(ResolutionManager resolutionManager)
    {
        _resolutionManager = resolutionManager;
    }

    public async Task RenderAsync()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║    ➕ Add New Resolution          ║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var resolution = new Resolution();

        // Title
        resolution.Title = InputValidator.GetRequiredString("Enter resolution title: ", 100);

        // Description
        Console.Write("Enter description (optional): ");
        resolution.Description = Console.ReadLine()?.Trim() ?? string.Empty;

        // Category
        resolution.Category = InputValidator.GetRequiredString("Enter category (e.g., Health, Career, Personal): ", 50);

        // Target Date
        resolution.TargetDate = InputValidator.GetOptionalDate("Enter target completion date (MM/DD/YYYY) or leave empty: ");

        // Reminder Settings
        ConfigureReminders(resolution);

        // Save
        try
        {
            await _resolutionManager.CreateResolutionAsync(resolution);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Resolution created successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ Error creating resolution: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void ConfigureReminders(Resolution resolution)
    {
        Console.WriteLine("\nReminder Configuration:");
        Console.WriteLine("  1. Interval-based (check in every X days)");
        Console.WriteLine("  2. Specific dates");
        Console.WriteLine("  3. Both");
        Console.WriteLine("  4. No reminders");

        var choice = InputValidator.GetChoice("Select reminder type: ", new[] { "1", "2", "3", "4" });

        switch (choice)
        {
            case "1":
                resolution.ReminderSettings.Type = ReminderType.Interval;
                resolution.ReminderSettings.IntervalDays = InputValidator.GetPositiveInteger("Check in every how many days? ");
                break;
            case "2":
                resolution.ReminderSettings.Type = ReminderType.SpecificDates;
                AddSpecificDates(resolution.ReminderSettings);
                break;
            case "3":
                resolution.ReminderSettings.Type = ReminderType.Both;
                resolution.ReminderSettings.IntervalDays = InputValidator.GetPositiveInteger("Check in every how many days? ");
                AddSpecificDates(resolution.ReminderSettings);
                break;
            case "4":
                // No reminders
                break;
        }
    }

    private void AddSpecificDates(ReminderSettings settings)
    {
        Console.WriteLine("\nEnter specific reminder dates (leave empty to finish):");

        while (true)
        {
            var date = InputValidator.GetOptionalDate($"Date #{settings.SpecificDates.Count + 1} (MM/DD/YYYY): ");
            if (!date.HasValue)
            {
                break;
            }
            settings.SpecificDates.Add(date.Value);
        }

        if (settings.SpecificDates.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️  No specific dates added.");
            Console.ResetColor();
        }
    }
}
