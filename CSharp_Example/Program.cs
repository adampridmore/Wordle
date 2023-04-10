public static class Program
{
  static readonly Api Api = new Api();

  static async Task Main()
  {
    try
    {
      if (!File.Exists("words.txt"))
      {
        await Api.DownloadWords("words.txt");
        Console.WriteLine("Downloaded word list");
      }

      var teamId = await Api.RegisterTeam("Example");
      Console.Write(teamId);

      var gameId = await Api.StartNewGame(teamId);
      Console.Write(gameId);
    }
    catch (Exception e)
    {
      Console.WriteLine("\nException Caught!");
      Console.WriteLine("Message :{0} ", e.Message);
    }
  }
}