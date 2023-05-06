
using Wordle.Models;
using Wordle;

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

      // var teamId = await Api.RegisterTeam("Example");
      // Console.WriteLine(teamId);

      var teamId = new System.Guid("d24641c0-0fec-457b-b257-19344790aad9"); // SeeSharpers

      var gameId = await Api.StartNewGame(teamId);
      Console.WriteLine("GameId: " + gameId);

      var firstGuess = "words";

      var newGuessResponse = await  Api.GuessWord(gameId, firstGuess);
      
      Console.WriteLine("newGuessResponse: " + newGuessResponse);

      var solver = new WordGuesser(File.ReadAllLines("words.txt").ToList());
    
      // TODO: Fix previousGuesses array
      var nextGuess = solver.GetNextGuess(firstGuess, newGuessResponse.Score, new String[]{});
      Console.WriteLine($"Score: {newGuessResponse.Score} NextGuess: {nextGuess}");
      
      while(newGuessResponse.State == GameState.InProgress){
        
        // TODO: Fix previousGuesses array
        nextGuess = solver.GetNextGuess(nextGuess, newGuessResponse.Score, new String[]{});

        newGuessResponse = await Api.GuessWord(gameId, nextGuess);

        Console.WriteLine($"Score: {newGuessResponse.Score} NextGuess: {nextGuess} State: {newGuessResponse.State}");
      } 
    }
    catch (Exception e)
    {
      Console.WriteLine("\nException Caught!");
      Console.WriteLine("Message :{0} ", e.Message);
    }
  }
}
