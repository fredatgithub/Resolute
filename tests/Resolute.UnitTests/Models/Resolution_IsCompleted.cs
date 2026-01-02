using ConsoleApp.Models;

namespace ConsoleApp.UnitTests.Models;

public class Resolution_IsCompleted
{
  [Fact]
  public void ReturnsFalse_GivenNullCompletedDate()
  {
    var resolution = new Resolution
    {
      CompletedDate = null
    };

    Assert.False(resolution.IsCompleted);
  }

  [Fact]
  public void ReturnsTrue_GivenCompletedDateHasValue()
  {
    var resolution = new Resolution
    {
      CompletedDate = DateTime.Now
    };

    Assert.True(resolution.IsCompleted);
  }

  [Fact]
  public void UpdatesWhenCompletedDateCleared_GivenPreviouslyCompleted()
  {
    var resolution = new Resolution
    {
      CompletedDate = DateTime.Now
    };

    resolution.CompletedDate = null;

    Assert.False(resolution.IsCompleted);
    Assert.Null(resolution.CompletedDate);
  }

  [Fact]
  public void UpdatesWhenCompletedDateSet_GivenNotCompleted()
  {
    var resolution = new Resolution();

    resolution.CompletedDate = DateTime.Now;

    Assert.True(resolution.IsCompleted);
    Assert.NotNull(resolution.CompletedDate);
  }
}
