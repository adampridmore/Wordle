using David_CSharp.Models;

public class NewGuessResponse
{
    public string Score { get; set; } = "";
    public GameState State { get; set; } = GameState.InProgress;

}