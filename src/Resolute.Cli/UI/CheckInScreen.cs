using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class CheckInScreen
{
  private readonly ResolutionManager _resolutionManager;
  private readonly ReminderService _reminderService;

  public CheckInScreen(ResolutionManager resolutionManager, ReminderService reminderService)
  {
    _resolutionManager = resolutionManager;
    _reminderService = reminderService;
  }

  public async Task RenderAsync()
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("╔════════════════════════════════════╗");
    Console.WriteLine("║       Check In on Resolution       ║");
    Console.WriteLine("╚════════════════════════════════════╝");
    Console.ResetColor();
    Console.WriteLine();

    var activeResolutions = _resolutionManager.GetActiveResolutions().ToList();

    if (!activeResolutions.Any())
    {
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine("📭 No active resolutions found.");
      Console.ResetColor();
      Console.WriteLine("\nPress any key to continue...");
      Console.ReadKey();
      return;
    }

    Console.WriteLine("Active Resolutions:\n");

    for (int i = 0; i < activeResolutions.Count; i++)
    {
      var resolution = activeResolutions[i];
      var lastCheckIn = resolution.CheckIns.OrderByDescending(c => c.Date).FirstOrDefault();
      var daysSinceCheckIn = lastCheckIn != null
          ? (DateTime.Now - lastCheckIn.Date).Days
          : (DateTime.Now - resolution.CreatedDate).Days;

      Console.WriteLine($"{i + 1}. {resolution.Title}");
      Console.ForegroundColor = ConsoleColor.Gray;
      Console.WriteLine($"   Category: {resolution.Category}");
      Console.WriteLine($"   Last check-in: {(lastCheckIn != null ? $"{daysSinceCheckIn} days ago" : "Never")}");
      Console.ResetColor();
      Console.WriteLine();
    }

    var choice = InputValidator.GetPositiveInteger("Select resolution to check in on: ", activeResolutions.Count);
    var selectedResolution = activeResolutions[choice - 1];

    await PerformCheckIn(selectedResolution);
  }

  private async Task PerformCheckIn(Resolution resolution)
  {
    Console.Clear();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"Check In: {resolution.Title}");
    Console.ResetColor();
    Console.WriteLine();

    var lastCheckIn = resolution.CheckIns.OrderByDescending(c => c.Date).FirstOrDefault();
    if (lastCheckIn != null)
    {
      var daysSince = (DateTime.Now - lastCheckIn.Date).Days;
      Console.WriteLine($"Last check-in: {lastCheckIn.Date:MM/dd/yyyy} ({daysSince} days ago)");
      Console.WriteLine($"Previous status: {lastCheckIn.Status}");
      if (!string.IsNullOrWhiteSpace(lastCheckIn.Notes))
      {
        Console.WriteLine($"Previous notes: {lastCheckIn.Notes}");
      }
      Console.WriteLine();
    }

    Console.WriteLine("How are you doing with this resolution?");
    Console.WriteLine("  1. On Track ✅");
    Console.WriteLine("  2. Struggling ⚠️");
    Console.WriteLine("  3. Completed 🎉");

    var statusChoice = InputValidator.GetChoice("Select status: ", new[] { "1", "2", "3" });

    var checkIn = new CheckIn
    {
      Date = DateTime.Now,
      Status = statusChoice switch
      {
        "1" => CheckInStatus.OnTrack,
        "2" => CheckInStatus.Struggling,
        "3" => CheckInStatus.Completed,
        _ => CheckInStatus.OnTrack
      }
    };

    Console.Write("\nAdd notes (optional): ");
    checkIn.Notes = Console.ReadLine()?.Trim() ?? string.Empty;

    await _resolutionManager.AddCheckInAsync(resolution.Id, checkIn);

    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("\n✅ Check-in recorded successfully!");
    Console.ResetColor();

    if (checkIn.Status == CheckInStatus.Completed)
    {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("\n🎉 Congratulations on completing your resolution! 🎉");
      Console.ResetColor();
    }
    else
    {
      var nextReminder = ReminderService.CalculateNextReminderDate(resolution);
      if (nextReminder.HasValue)
      {
        Console.WriteLine($"\nNext reminder: {nextReminder.Value:MM/dd/yyyy}");
      }
    }

    Console.WriteLine("\nPress any key to continue...");
    Console.ReadKey();
  }
}
