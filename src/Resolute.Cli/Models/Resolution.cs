using System;
using System.Collections.Generic;

namespace ConsoleApp.Models;

public class Resolution
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? TargetDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool IsCompleted => CompletedDate.HasValue;
    public ReminderSettings ReminderSettings { get; set; } = new();
    public List<CheckIn> CheckIns { get; set; } = new();
}
