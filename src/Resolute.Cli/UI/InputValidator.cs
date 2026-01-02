using System;
using System.Globalization;
using System.Linq;

namespace ConsoleApp.UI;

public static class InputValidator
{
    public static string GetRequiredString(string prompt, int maxLength = 200)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ This field is required. Please enter a value.");
                Console.ResetColor();
                continue;
            }

            if (input.Length > maxLength)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Input is too long. Maximum {maxLength} characters allowed.");
                Console.ResetColor();
                continue;
            }

            return input;
        }
    }

    public static DateTime? GetOptionalDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (DateTime.TryParse(input, out var date))
            {
                return date;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid date format. Please use MM/DD/YYYY.");
            Console.ResetColor();
        }
    }

    public static DateTime GetRequiredDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ This field is required. Please enter a date.");
                Console.ResetColor();
                continue;
            }

            if (DateTime.TryParse(input, out var date))
            {
                return date;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ Invalid date format. Please use MM/DD/YYYY.");
            Console.ResetColor();
        }
    }

    public static int GetPositiveInteger(string prompt, int? max = null)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (!int.TryParse(input, out var number) || number <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Please enter a positive number.");
                Console.ResetColor();
                continue;
            }

            if (max.HasValue && number > max.Value)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Number must be {max.Value} or less.");
                Console.ResetColor();
                continue;
            }

            return number;
        }
    }

    public static string GetChoice(string prompt, string[] validChoices)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (validChoices.Contains(input, StringComparer.OrdinalIgnoreCase))
            {
                return input;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Invalid choice. Please enter one of: {string.Join(", ", validChoices)}");
            Console.ResetColor();
        }
    }

    public static bool GetYesNo(string prompt)
    {
        var choice = GetChoice(prompt + " (y/n): ", new[] { "y", "n", "yes", "no" });
        return choice.StartsWith("y", StringComparison.OrdinalIgnoreCase);
    }
}
