using ConsoleApp.Models;

namespace Resolute.UnitTests.Models;

public class Resolution_Constructor
{
    [Fact]
    public void CreatesNewGuid_GivenDefaultConstructor()
    {
        var resolution = new Resolution();

        Assert.NotEqual(Guid.Empty, resolution.Id);
    }

    [Fact]
    public void InitializesEmptyStrings_GivenDefaultConstructor()
    {
        var resolution = new Resolution();

        Assert.Equal(string.Empty, resolution.Title);
        Assert.Equal(string.Empty, resolution.Description);
        Assert.Equal(string.Empty, resolution.Category);
    }

    [Fact]
    public void SetsCreatedDateToNow_GivenDefaultConstructor()
    {
        var resolution = new Resolution();

        Assert.True(resolution.CreatedDate <= DateTime.Now);
        Assert.True(resolution.CreatedDate >= DateTime.Now.AddSeconds(-1));
    }

    [Fact]
    public void InitializesNullDates_GivenDefaultConstructor()
    {
        var resolution = new Resolution();

        Assert.Null(resolution.TargetDate);
        Assert.Null(resolution.CompletedDate);
    }

    [Fact]
    public void InitializesReminderSettings_GivenDefaultConstructor()
    {
        var resolution = new Resolution();

        Assert.NotNull(resolution.ReminderSettings);
    }

    [Fact]
    public void InitializesEmptyCheckInsList_GivenDefaultConstructor()
    {
        var resolution = new Resolution();

        Assert.NotNull(resolution.CheckIns);
        Assert.Empty(resolution.CheckIns);
    }
}
