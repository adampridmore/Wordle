public class GetGameResponse
{
  public class GetGameScoreResponse
  {
    public string? Guess { get; set; }
    public string? Score { get; set; }
  }
  
  public Guid GameId { get; set; }
  public string Word { get; set; } = "";  // Only populated when state!=inprogress
  public GameState State { get; set; } = GameState.InProgress;
  public GetGameScoreResponse[] Guesses { get; set; } = new GetGameScoreResponse[] { };
}