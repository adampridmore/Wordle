namespace Wordle.Models;

public class NewGuessResponse
{
    override public string ToString() {
        return $"Score: [{Score}] State: {State}";
    }

    public string Score { get; set; } = "";
    public GameState State { get; set; } = GameState.InProgress;

}
