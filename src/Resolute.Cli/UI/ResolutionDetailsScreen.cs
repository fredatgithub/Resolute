using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class ResolutionDetailsScreen
{
  private readonly ResolutionManager _resolutionManager;
  private Resolution _resolution;

  public ResolutionDetailsScreen(ResolutionManager resolutionManager, Resolution resolution)
  {
    _resolutionManager = resolutionManager;
    _resolution = resolution;
  }

  public async Task RenderAsync()
  {
    Console.Clear();
    DisplayResolutionDetails();

    Console.WriteLine("\nActions:");
    Console.WriteLine("  1. Edit Resolution");
    Console.WriteLine("  2. Mark as Complete");
    Console.WriteLine("  3. Delete Resolution");
    Console.WriteLine("  4. Back to List");

    var choice = InputValidator.GetChoice("Select action: ", new[] { "1", "2", "3", "4" });

    switch (choice)
    {
      case "1":
        await new EditResolutionScreen(_resolutionManager, _resolution).RenderAsync();
        break;
      case "2":
        await MarkAsComplete();
        break;
      case "3":
        await DeleteResolution();
        break;
    }
  }

  private void DisplayResolutionDetails()
  {
    var statusIcon = _resolution.IsCompleted ? "✅" : "⏳";
    var statusText = _resolution.IsCompleted ? "COMPLETED" : "ACTIVE";

    Console.ForegroundColor = _resolution.IsCompleted ? ConsoleColor.Green : ConsoleColor.Yellow;
    Console.WriteLine($"{statusIcon} {statusText}");
    Console.ResetColor();
    Console.WriteLine();

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"Title: {_resolution.Title}");
    Console.ResetColor();

    if (!string.IsNullOrWhiteSpace(_resolution.Description))
    {
      Console.WriteLine($"\nDescription: {_resolution.Description}");
    }

    Console.WriteLine($"\nCategory: {_resolution.Category}");
    Console.WriteLine($"Created: {_resolution.CreatedDate:MM/dd/yyyy}");

    if (_resolution.TargetDate.HasValue)
    {
      Console.WriteLine($"Target Date: {_resolution.TargetDate.Value:MM/dd/yyyy}");
      var daysRemaining = (_resolution.TargetDate.Value - DateTime.Now).Days;
      if (daysRemaining > 0)
      {
        Console.WriteLine($"Days Remaining: {daysRemaining}");
      }
      else if (daysRemaining == 0)
      {
        Console.WriteLine("Target Date: TODAY!");
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Overdue by: {Math.Abs(daysRemaining)} days");
        Console.ResetColor();
      }
    }

    if (_resolution.IsCompleted && _resolution.CompletedDate.HasValue)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine($"Completed: {_resolution.CompletedDate.Value:MM/dd/yyyy}");
      Console.ResetColor();
    }

    // Reminder settings
    Console.WriteLine("\nReminder Settings:");
    if (_resolution.ReminderSettings.Type == ReminderType.Interval || _resolution.ReminderSettings.Type == ReminderType.Both)
    {
      Console.WriteLine($"  • Check in every {_resolution.ReminderSettings.IntervalDays} days");
    }
    if (_resolution.ReminderSettings.Type == ReminderType.SpecificDates || _resolution.ReminderSettings.Type == ReminderType.Both)
    {
      if (_resolution.ReminderSettings.SpecificDates.Any())
      {
        Console.WriteLine("  • Specific dates:");
        foreach (var date in _resolution.ReminderSettings.SpecificDates.OrderBy(d => d))
        {
          Console.WriteLine($"    - {date:MM/dd/yyyy}");
        }
      }
    }

    // Check-ins
    if (_resolution.CheckIns.Any())
    {
      Console.WriteLine($"\nCheck-In History ({_resolution.CheckIns.Count} total):");
      foreach (var checkIn in _resolution.CheckIns.OrderByDescending(c => c.Date).Take(5))
      {
        var statusColor = checkIn.Status switch
        {
          CheckInStatus.Completed => ConsoleColor.Green,
          CheckInStatus.OnTrack => ConsoleColor.Cyan,
          CheckInStatus.Struggling => ConsoleColor.Yellow,
          _ => ConsoleColor.White
        };

        Console.ForegroundColor = statusColor;
        Console.Write($"  • {checkIn.Date:MM/dd/yyyy} - {checkIn.Status}");
        Console.ResetColor();

        if (!string.IsNullOrWhiteSpace(checkIn.Notes))
        {
          Console.WriteLine($": {checkIn.Notes}");
        }
        else
        {
          Console.WriteLine();
        }
      }
    }
    else
    {
      Console.WriteLine("\nNo check-ins yet.");
    }
  }

  private async Task MarkAsComplete()
  {
    if (_resolution.IsCompleted)
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("\n⚠️  This resolution is already marked as complete.");
      Console.ResetColor();
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
      return;
    }

    if (InputValidator.GetYesNo("\nAre you sure you want to mark this as complete?"))
    {
      await _resolutionManager.CompleteResolutionAsync(_resolution.Id);
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("\n🎉 Congratulations! Resolution marked as complete!");
      Console.ResetColor();
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }
  }

  private async Task DeleteResolution()
  {
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("\n⚠️  WARNING: This action cannot be undone!");
    Console.ResetColor();

    if (InputValidator.GetYesNo("Are you sure you want to delete this resolution?"))
    {
      await _resolutionManager.DeleteResolutionAsync(_resolution.Id);
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("\n🗑️  Resolution deleted.");
      Console.ResetColor();
      Console.WriteLine("Press any key to continue...");
      Console.ReadKey();
    }
  }
}
