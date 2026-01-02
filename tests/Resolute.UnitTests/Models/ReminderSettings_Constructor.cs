using ConsoleApp.Models;

namespace ConsoleApp.UnitTests.Models;

public class ReminderSettings_Constructor
{
    [Fact]
    public void InitializesDefaultType_GivenDefaultConstructor()
    {
        var settings = new ReminderSettings();

        Assert.Equal(default(ReminderType), settings.Type);
    }

    [Fact]
    public void InitializesNullIntervalDays_GivenDefaultConstructor()
    {
        var settings = new ReminderSettings();

        Assert.Null(settings.IntervalDays);
    }

    [Fact]
    public void InitializesEmptySpecificDates_GivenDefaultConstructor()
    {
        var settings = new ReminderSettings();

        Assert.NotNull(settings.SpecificDates);
        Assert.Empty(settings.SpecificDates);
    }
}
