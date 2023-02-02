public static class Program
{
  static readonly API api = new API();

  static async Task Main()
  {
    try
    {
      var teamId = await api.RegisterTeam("Davids Example");
      Console.Write(teamId);
      
      var gameId = await api.StartNewGame(teamId);
      Console.Write(gameId);

      var guess = await api.GuessWord(gameId, "ABOUT");
      Console.WriteLine(guess.Score);
      Console.WriteLine(guess.State.ToString());

      var game = await api.GetGame(gameId);
      Console.WriteLine(game.State.ToString());
    }
    catch (Exception e)
    {
      Console.WriteLine("\nException Caught!");
      Console.WriteLine("Message :{0} ", e.Message);
    }
  }
}