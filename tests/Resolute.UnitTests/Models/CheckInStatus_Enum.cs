using ConsoleApp.Models;

namespace Resolute.UnitTests.Models;

public class CheckInStatus_Enum
{
    [Fact]
    public void HasThreeValues_GivenEnumDefinition()
    {
        var values = Enum.GetValues<CheckInStatus>();

        Assert.Equal(3, values.Length);
        Assert.Contains(CheckInStatus.OnTrack, values);
        Assert.Contains(CheckInStatus.Struggling, values);
        Assert.Contains(CheckInStatus.Completed, values);
    }

    [Theory]
    [InlineData(CheckInStatus.OnTrack, "OnTrack")]
    [InlineData(CheckInStatus.Struggling, "Struggling")]
    [InlineData(CheckInStatus.Completed, "Completed")]
    public void ReturnsCorrectName_GivenEnumValue(CheckInStatus status, string expectedName)
    {
        var actualName = status.ToString();

        Assert.Equal(expectedName, actualName);
    }
}
