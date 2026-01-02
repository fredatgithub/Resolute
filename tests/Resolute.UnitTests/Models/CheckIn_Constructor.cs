using ConsoleApp.Models;

namespace ConsoleApp.UnitTests.Models;

public class CheckIn_Constructor
{
    [Fact]
    public void InitializesDefaultDateTime_GivenDefaultConstructor()
    {
        var checkIn = new CheckIn();

        Assert.Equal(default(DateTime), checkIn.Date);
    }

    [Fact]
    public void InitializesDefaultStatus_GivenDefaultConstructor()
    {
        var checkIn = new CheckIn();

        Assert.Equal(default(CheckInStatus), checkIn.Status);
    }

    [Fact]
    public void InitializesEmptyNotes_GivenDefaultConstructor()
    {
        var checkIn = new CheckIn();

        Assert.Equal(string.Empty, checkIn.Notes);
    }
}
