using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;
using ConsoleApp.Services;

namespace ConsoleApp.UI;

public class ViewResolutionsScreen
{
    private readonly ResolutionManager _resolutionManager;

    public ViewResolutionsScreen(ResolutionManager resolutionManager)
    {
        _resolutionManager = resolutionManager;
    }

    public async Task RenderAsync()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║    📋 All Resolutions              ║");
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine("Filter by:");
        Console.WriteLine("  1. All resolutions");
        Console.WriteLine("  2. Active only");
        Console.WriteLine("  3. Completed only");
        Console.WriteLine("  4. By category");

        var choice = InputValidator.GetChoice("Select filter: ", new[] { "1", "2", "3", "4" });

        IEnumerable<Resolution> resolutions = choice switch
        {
            "1" => _resolutionManager.GetAllResolutions(),
            "2" => _resolutionManager.GetActiveResolutions(),
            "3" => _resolutionManager.GetCompletedResolutions(),
            "4" => GetResolutionsByCategory(),
            _ => _resolutionManager.GetAllResolutions()
        };

        DisplayResolutions(resolutions);

        Console.WriteLine("\nOptions:");
        Console.WriteLine("  Enter resolution number to view details");
        Console.WriteLine("  Press Enter to return to main menu");
        Console.Write("\nYour choice: ");

        var input = Console.ReadLine()?.Trim() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(input) && int.TryParse(input, out var index))
        {
            var resolutionsList = resolutions.ToList();
            if (index > 0 && index <= resolutionsList.Count)
            {
                await new ResolutionDetailsScreen(_resolutionManager, resolutionsList[index - 1]).RenderAsync();
            }
        }
    }

    private IEnumerable<Resolution> GetResolutionsByCategory()
    {
        var categories = _resolutionManager.GetAllResolutions()
            .Select(r => r.Category)
            .Distinct()
            .OrderBy(c => c)
            .ToList();

        if (!categories.Any())
        {
            Console.WriteLine("No categories found.");
            return Enumerable.Empty<Resolution>();
        }

        Console.WriteLine("\nAvailable categories:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {categories[i]}");
        }

        var choice = InputValidator.GetPositiveInteger("Select category: ", categories.Count);
        return _resolutionManager.GetResolutionsByCategory(categories[choice - 1]);
    }

    private void DisplayResolutions(IEnumerable<Resolution> resolutions)
    {
        var resolutionsList = resolutions.ToList();

        if (!resolutionsList.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n📭 No resolutions found.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine($"\nFound {resolutionsList.Count} resolution(s):\n");

        for (int i = 0; i < resolutionsList.Count; i++)
        {
            var resolution = resolutionsList[i];
            var statusIcon = resolution.IsCompleted ? "✅" : "⏳";
            var targetInfo = resolution.TargetDate.HasValue
                ? $" (Target: {resolution.TargetDate.Value:MM/dd/yyyy})"
                : string.Empty;

            Console.Write($"{i + 1}. {statusIcon} ");

            if (resolution.IsCompleted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine($"{resolution.Title}{targetInfo}");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"   Category: {resolution.Category} | Created: {resolution.CreatedDate:MM/dd/yyyy}");
            Console.ResetColor();

            if (!string.IsNullOrWhiteSpace(resolution.Description))
            {
                var shortDesc = resolution.Description.Length > 60
                    ? resolution.Description.Substring(0, 57) + "..."
                    : resolution.Description;
                Console.WriteLine($"   {shortDesc}");
            }
            Console.WriteLine();
        }
    }
}
