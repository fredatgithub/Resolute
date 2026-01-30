namespace ConsoleApp.Models;

public class CheckIn
{
  public DateTime Date { get; set; }
  public CheckInStatus Status { get; set; }
  public string Notes { get; set; } = string.Empty;
}

public enum CheckInStatus
{
  OnTrack,
  Struggling,
  Completed
}
