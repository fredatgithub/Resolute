using ConsoleApp.Models;

namespace Resolute.UnitTests.Models;

public class ReminderType_Enum
{
    [Fact]
    public void HasThreeValues_GivenEnumDefinition()
    {
        var values = Enum.GetValues<ReminderType>();

        Assert.Equal(3, values.Length);
        Assert.Contains(ReminderType.Interval, values);
        Assert.Contains(ReminderType.SpecificDates, values);
        Assert.Contains(ReminderType.Both, values);
    }

    [Theory]
    [InlineData(ReminderType.Interval, "Interval")]
    [InlineData(ReminderType.SpecificDates, "SpecificDates")]
    [InlineData(ReminderType.Both, "Both")]
    public void ReturnsCorrectName_GivenEnumValue(ReminderType type, string expectedName)
    {
        var actualName = type.ToString();

        Assert.Equal(expectedName, actualName);
    }
}
