using ConsoleApp.Models;

namespace ConsoleApp.UnitTests.Models;

public class CheckIn_Properties
{
    [Fact]
    public void SetsAllProperties_GivenValidValues()
    {
        var date = new DateTime(2026, 1, 15);
        var status = CheckInStatus.OnTrack;
        var notes = "Making good progress!";

        var checkIn = new CheckIn
        {
            Date = date,
            Status = status,
            Notes = notes
        };

        Assert.Equal(date, checkIn.Date);
        Assert.Equal(status, checkIn.Status);
        Assert.Equal(notes, checkIn.Notes);
    }

    [Theory]
    [InlineData(CheckInStatus.OnTrack)]
    [InlineData(CheckInStatus.Struggling)]
    [InlineData(CheckInStatus.Completed)]
    public void SetsStatus_GivenAllValidStatusValues(CheckInStatus status)
    {
        var checkIn = new CheckIn { Status = status };

        Assert.Equal(status, checkIn.Status);
    }

    [Fact]
    public void SetsEmptyNotes_GivenEmptyString()
    {
        var checkIn = new CheckIn { Notes = string.Empty };

        Assert.Equal(string.Empty, checkIn.Notes);
    }

    [Fact]
    public void SetsLongNotes_GivenLargeText()
    {
        var longNotes = new string('a', 1000);

        var checkIn = new CheckIn { Notes = longNotes };

        Assert.Equal(longNotes, checkIn.Notes);
        Assert.Equal(1000, checkIn.Notes.Length);
    }
}
