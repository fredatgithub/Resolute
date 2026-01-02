using System;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class EditResolutionScreen
{
    private readonly ResolutionManager _resolutionManager;
    private readonly Resolution _resolution;

    public EditResolutionScreen(ResolutionManager resolutionManager, Resolution resolution)
    {
        _resolutionManager = resolutionManager;
        _resolution = resolution;
    }

    public async Task RenderAsync()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║    ✏️  Edit Resolution             ║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine("Leave fields empty to keep current values.\n");

        // Title
        Console.WriteLine($"Current title: {_resolution.Title}");
        Console.Write("New title (or press Enter to keep): ");
        var title = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(title))
        {
            _resolution.Title = title;
        }

        // Description
        Console.WriteLine($"\nCurrent description: {_resolution.Description}");
        Console.Write("New description (or press Enter to keep): ");
        var description = Console.ReadLine()?.Trim();
        if (description != null)
        {
            _resolution.Description = description;
        }

        // Category
        Console.WriteLine($"\nCurrent category: {_resolution.Category}");
        Console.Write("New category (or press Enter to keep): ");
        var category = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(category))
        {
            _resolution.Category = category;
        }

        // Target Date
        Console.WriteLine($"\nCurrent target date: {(_resolution.TargetDate?.ToString("MM/dd/yyyy") ?? "None")}");
        Console.Write("New target date (MM/DD/YYYY or press Enter to keep): ");
        var targetDateInput = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(targetDateInput) && DateTime.TryParse(targetDateInput, out var targetDate))
        {
            _resolution.TargetDate = targetDate;
        }

        // Update reminders
        if (InputValidator.GetYesNo("\nUpdate reminder settings?"))
        {
            ConfigureReminders(_resolution);
        }

        // Save
        try
        {
            await _resolutionManager.UpdateResolutionAsync(_resolution);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Resolution updated successfully!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ Error updating resolution: {ex.Message}");
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

        resolution.ReminderSettings = new ReminderSettings();

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
    }
}
