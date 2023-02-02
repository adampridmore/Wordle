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

    }
    catch (HttpRequestException e)
    {
      Console.WriteLine("\nException Caught!");
      Console.WriteLine("Message :{0} ", e.Message);
    }
  }
}