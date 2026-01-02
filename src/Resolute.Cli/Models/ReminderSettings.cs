using System;
using System.Collections.Generic;

namespace ConsoleApp.Models;

public class ReminderSettings
{
    public ReminderType Type { get; set; }
    public int? IntervalDays { get; set; }
    public List<DateTime> SpecificDates { get; set; } = new();
}

public enum ReminderType
{
    Interval,
    SpecificDates,
    Both
}
