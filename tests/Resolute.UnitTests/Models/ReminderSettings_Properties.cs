using ConsoleApp.Models;

namespace ConsoleApp.UnitTests.Models;

public class ReminderSettings_Properties
{
    [Fact]
    public void SetsIntervalType_GivenIntervalDays()
    {
        var intervalDays = 7;

        var settings = new ReminderSettings
        {
            Type = ReminderType.Interval,
            IntervalDays = intervalDays
        };

        Assert.Equal(ReminderType.Interval, settings.Type);
        Assert.Equal(intervalDays, settings.IntervalDays);
    }

    [Fact]
    public void SetsSpecificDatesType_GivenDateList()
    {
        var dates = new List<DateTime>
        {
            new DateTime(2026, 2, 1),
            new DateTime(2026, 3, 1),
            new DateTime(2026, 4, 1)
        };

        var settings = new ReminderSettings
        {
            Type = ReminderType.SpecificDates,
            SpecificDates = dates
        };

        Assert.Equal(ReminderType.SpecificDates, settings.Type);
        Assert.Equal(3, settings.SpecificDates.Count);
        Assert.Equal(dates, settings.SpecificDates);
    }

    [Fact]
    public void SetsBothType_GivenIntervalAndDates()
    {
        var intervalDays = 14;
        var dates = new List<DateTime> { new DateTime(2026, 6, 1) };

        var settings = new ReminderSettings
        {
            Type = ReminderType.Both,
            IntervalDays = intervalDays,
            SpecificDates = dates
        };

        Assert.Equal(ReminderType.Both, settings.Type);
        Assert.Equal(intervalDays, settings.IntervalDays);
        Assert.Single(settings.SpecificDates);
        Assert.Contains(dates[0], settings.SpecificDates);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    [InlineData(14)]
    [InlineData(30)]
    [InlineData(365)]
    public void SetsVariousIntervalDays_GivenValidDayValues(int days)
    {
        var settings = new ReminderSettings
        {
            Type = ReminderType.Interval,
            IntervalDays = days
        };

        Assert.Equal(days, settings.IntervalDays);
    }

    [Fact]
    public void AddsMultipleDates_GivenSpecificDatesType()
    {
        var settings = new ReminderSettings { Type = ReminderType.SpecificDates };

        settings.SpecificDates.Add(new DateTime(2026, 1, 15));
        settings.SpecificDates.Add(new DateTime(2026, 2, 15));
        settings.SpecificDates.Add(new DateTime(2026, 3, 15));

        Assert.Equal(3, settings.SpecificDates.Count);
    }

    [Fact]
    public void AllowsEmptySpecificDates_GivenEmptyList()
    {
        var settings = new ReminderSettings
        {
            Type = ReminderType.SpecificDates,
            SpecificDates = new List<DateTime>()
        };

        Assert.Empty(settings.SpecificDates);
    }

    [Fact]
    public void AllowsNullIntervalDays_GivenSpecificDatesType()
    {
        var settings = new ReminderSettings
        {
            Type = ReminderType.SpecificDates,
            IntervalDays = null
        };

        Assert.Null(settings.IntervalDays);
    }
}
