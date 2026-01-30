using ConsoleApp.Models;

namespace Resolute.UnitTests.Models;

public class ResolutionData_Constructor
{
    [Fact]
    public void InitializesEmptyList_GivenDefaultConstructor()
    {
        var data = new ResolutionData();

        Assert.NotNull(data.Resolutions);
        Assert.Empty(data.Resolutions);
    }
}
