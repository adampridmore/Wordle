public class GetGameResponse
{
  public class GetGameScoreResponse
  {
    public string? Guess { get; set; }
    public string? Score { get; set; }
  }
  
  public Guid GameId { get; set; }
  public string Word { get; set; } = string.Empty;
  public DateTime DateStarted { get; set; }
  public GameState State { get; init; }
  public GetGameScoreResponse[] Guesses { get; set; } = new GetGameScoreResponse[] { };
}