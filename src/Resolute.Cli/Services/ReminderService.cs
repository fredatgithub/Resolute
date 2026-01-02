using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Models;

namespace ConsoleApp.Services;

public class ReminderService
{
    private readonly ResolutionManager _resolutionManager;

    public ReminderService(ResolutionManager resolutionManager)
    {
        _resolutionManager = resolutionManager;
    }

    public async Task<IEnumerable<Resolution>> GetDueRemindersAsync()
    {
        var activeResolutions = _resolutionManager.GetActiveResolutions();
        var dueResolutions = new List<Resolution>();

        foreach (var resolution in activeResolutions)
        {
            if (await IsDueForCheckInAsync(resolution))
            {
                dueResolutions.Add(resolution);
            }
        }

        return dueResolutions;
    }

    public async Task<bool> IsDueForCheckInAsync(Resolution resolution)
    {
        var lastCheckIn = resolution.CheckIns.OrderByDescending(c => c.Date).FirstOrDefault();
        var lastCheckInDate = lastCheckIn?.Date ?? resolution.CreatedDate;

        if (resolution.ReminderSettings.Type == ReminderType.Interval ||
            resolution.ReminderSettings.Type == ReminderType.Both)
        {
            if (resolution.ReminderSettings.IntervalDays.HasValue)
            {
                var daysSinceLastCheckIn = (DateTime.Now - lastCheckInDate).Days;
                if (daysSinceLastCheckIn >= resolution.ReminderSettings.IntervalDays.Value)
                {
                    return true;
                }
            }
        }

        if (resolution.ReminderSettings.Type == ReminderType.SpecificDates ||
            resolution.ReminderSettings.Type == ReminderType.Both)
        {
            var upcomingDates = resolution.ReminderSettings.SpecificDates
                .Where(d => d.Date <= DateTime.Now.Date && d.Date > lastCheckInDate.Date);

            if (upcomingDates.Any())
            {
                return true;
            }
        }

        return false;
    }

    public DateTime? CalculateNextReminderDate(Resolution resolution)
    {
        var lastCheckIn = resolution.CheckIns.OrderByDescending(c => c.Date).FirstOrDefault();
        var lastCheckInDate = lastCheckIn?.Date ?? resolution.CreatedDate;

        DateTime? nextIntervalDate = null;
        DateTime? nextSpecificDate = null;

        if (resolution.ReminderSettings.Type == ReminderType.Interval ||
            resolution.ReminderSettings.Type == ReminderType.Both)
        {
            if (resolution.ReminderSettings.IntervalDays.HasValue)
            {
                nextIntervalDate = lastCheckInDate.AddDays(resolution.ReminderSettings.IntervalDays.Value);
            }
        }

        if (resolution.ReminderSettings.Type == ReminderType.SpecificDates ||
            resolution.ReminderSettings.Type == ReminderType.Both)
        {
            nextSpecificDate = resolution.ReminderSettings.SpecificDates
                .Where(d => d > lastCheckInDate)
                .OrderBy(d => d)
                .FirstOrDefault();
        }

        if (nextIntervalDate.HasValue && nextSpecificDate.HasValue)
        {
            return nextIntervalDate < nextSpecificDate ? nextIntervalDate : nextSpecificDate;
        }

        return nextIntervalDate ?? nextSpecificDate;
    }

    public async Task<IEnumerable<(Resolution Resolution, DateTime ReminderDate)>> GetUpcomingRemindersAsync(int days = 7)
    {
        var activeResolutions = _resolutionManager.GetActiveResolutions();
        var upcomingReminders = new List<(Resolution, DateTime)>();
        var endDate = DateTime.Now.AddDays(days);

        foreach (var resolution in activeResolutions)
        {
            var nextReminder = CalculateNextReminderDate(resolution);
            if (nextReminder.HasValue && nextReminder.Value <= endDate)
            {
                upcomingReminders.Add((resolution, nextReminder.Value));
            }
        }

        return upcomingReminders.OrderBy(r => r.Item2);
    }
}
