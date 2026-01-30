using ConsoleApp.Models;

namespace Resolute.UnitTests.Models;

public class Resolution_Properties
{
    [Fact]
    public void SetsAllProperties_GivenValidValues()
    {
        var id = Guid.NewGuid();
        var title = "Test Resolution";
        var description = "Test Description";
        var category = "Health";
        var createdDate = new DateTime(2026, 1, 1);
        var targetDate = new DateTime(2026, 12, 31);
        var completedDate = new DateTime(2026, 6, 15);
        var reminderSettings = new ReminderSettings { Type = ReminderType.Interval, IntervalDays = 7 };
        var checkIns = new List<CheckIn>
        {
            new CheckIn { Date = DateTime.Now, Status = CheckInStatus.OnTrack }
        };

        var resolution = new Resolution
        {
            Id = id,
            Title = title,
            Description = description,
            Category = category,
            CreatedDate = createdDate,
            TargetDate = targetDate,
            CompletedDate = completedDate,
            ReminderSettings = reminderSettings,
            CheckIns = checkIns
        };

        Assert.Equal(id, resolution.Id);
        Assert.Equal(title, resolution.Title);
        Assert.Equal(description, resolution.Description);
        Assert.Equal(category, resolution.Category);
        Assert.Equal(createdDate, resolution.CreatedDate);
        Assert.Equal(targetDate, resolution.TargetDate);
        Assert.Equal(completedDate, resolution.CompletedDate);
        Assert.True(resolution.IsCompleted);
        Assert.Same(reminderSettings, resolution.ReminderSettings);
        Assert.Same(checkIns, resolution.CheckIns);
    }

    [Fact]
    public void AddsCheckIns_GivenMultipleItems()
    {
        var resolution = new Resolution();
        var checkIn1 = new CheckIn { Status = CheckInStatus.OnTrack };
        var checkIn2 = new CheckIn { Status = CheckInStatus.Struggling };

        resolution.CheckIns.Add(checkIn1);
        resolution.CheckIns.Add(checkIn2);

        Assert.Equal(2, resolution.CheckIns.Count);
        Assert.Contains(checkIn1, resolution.CheckIns);
        Assert.Contains(checkIn2, resolution.CheckIns);
    }
}
