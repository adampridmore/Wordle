using David_CSharp.Models;

public class GetGamesResponse
{
  public Guid GameId { get; set; }
  public DateTime DateStarted { get; set; }
  public GameState State { get; init; }
}