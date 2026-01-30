using ConsoleApp.Services;
using ConsoleApp.UI;

try
{
  // Initialize services
  var dataService = new JsonDataService("resolutions.json");
  var resolutionManager = new ResolutionManager(dataService);
  await resolutionManager.InitializeAsync();

  var reminderService = new ReminderService(resolutionManager);
  var statisticsService = new StatisticsService(resolutionManager);

  // Start the application
  var mainMenu = new MainMenu(resolutionManager, reminderService, statisticsService);
  await mainMenu.RenderAsync();
}
catch (Exception ex)
{
  Console.ForegroundColor = ConsoleColor.Red;
  Console.WriteLine($"\n❌ An error occurred: {ex.Message}");
  Console.ResetColor();
  Console.WriteLine("\nPress any key to exit...");
  Console.ReadKey();
}
