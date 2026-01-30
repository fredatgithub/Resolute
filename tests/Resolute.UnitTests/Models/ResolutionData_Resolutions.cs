using ConsoleApp.Models;

namespace Resolute.UnitTests.Models;

public class ResolutionData_Resolutions
{
  [Fact]
  public void AddsResolutions_GivenMultipleItems()
  {
    var data = new ResolutionData();
    var resolution1 = new Resolution { Title = "Resolution 1" };
    var resolution2 = new Resolution { Title = "Resolution 2" };

    data.Resolutions.Add(resolution1);
    data.Resolutions.Add(resolution2);

    Assert.Equal(2, data.Resolutions.Count);
    Assert.Contains(resolution1, data.Resolutions);
    Assert.Contains(resolution2, data.Resolutions);
  }

  [Fact]
  public void RemovesResolution_GivenExistingItem()
  {
    var data = new ResolutionData();
    var resolution1 = new Resolution { Title = "Resolution 1" };
    var resolution2 = new Resolution { Title = "Resolution 2" };
    data.Resolutions.Add(resolution1);
    data.Resolutions.Add(resolution2);

    data.Resolutions.Remove(resolution1);

    Assert.Single(data.Resolutions);
    Assert.DoesNotContain(resolution1, data.Resolutions);
    Assert.Contains(resolution2, data.Resolutions);
  }

  [Fact]
  public void ClearsAllResolutions_GivenMultipleItems()
  {
    var data = new ResolutionData();
    data.Resolutions.Add(new Resolution { Title = "Resolution 1" });
    data.Resolutions.Add(new Resolution { Title = "Resolution 2" });
    data.Resolutions.Add(new Resolution { Title = "Resolution 3" });

    data.Resolutions.Clear();

    Assert.Empty(data.Resolutions);
  }

  [Fact]
  public void SupportsLinqQueries_GivenMultipleResolutions()
  {
    var data = new ResolutionData();
    data.Resolutions.Add(new Resolution { Title = "Health Goal", Category = "Health" });
    data.Resolutions.Add(new Resolution { Title = "Career Goal", Category = "Career" });
    data.Resolutions.Add(new Resolution { Title = "Fitness Goal", Category = "Health" });

    var healthResolutions = data.Resolutions.Where(r => r.Category == "Health").ToList();

    Assert.Equal(2, healthResolutions.Count);
    Assert.All(healthResolutions, r => Assert.Equal("Health", r.Category));
  }

  [Fact]
  public void ReplacesListReference_GivenNewList()
  {
    var data = new ResolutionData();
    var newList = new List<Resolution>
        {
            new Resolution { Title = "Resolution 1" },
            new Resolution { Title = "Resolution 2" }
        };

    data.Resolutions = newList;

    Assert.Same(newList, data.Resolutions);
    Assert.Equal(2, data.Resolutions.Count);
  }

  [Fact]
  public void HandlesManyItems_GivenLargeCollection()
  {
    var data = new ResolutionData();

    for (var i = 0; i < 100; i++)
    {
      data.Resolutions.Add(new Resolution { Title = $"Resolution {i}" });
    }

    Assert.Equal(100, data.Resolutions.Count);
  }
}
