using David_CSharp.Models;

namespace David_CSharp;

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

      var words = await File.ReadAllLinesAsync("words.txt");

      var teamId = await Api.RegisterTeam("Davids Example");
      Console.Write(teamId);

      var gameId = await Api.StartNewGame(teamId);
      Console.Write(gameId);

      // Pick a word at random
      var rnd = new Random();
      var game = await Api.GetGame(gameId);
      
      while (game.State == GameState.InProgress)
      {
        var wordToTry = words[rnd.NextInt64(words.LongCount())];

        var guess = await Api.GuessWord(gameId, wordToTry);
        Console.WriteLine(wordToTry);
        Console.WriteLine(guess.Score);
        Console.WriteLine(guess.State.ToString());

        words = FilterUsingScore(words, wordToTry, guess.Score);

        game = await Api.GetGame(gameId);
        Console.WriteLine(game.State.ToString());
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("\nException Caught!");
      Console.WriteLine("Message :{0} ", e.Message);
    }
  }

  private static string[] FilterUsingScore(string[] words, string guess, string score)
  {
    return words.Where(word => IsValid(word, guess, score)).ToArray();
  }

  private static bool IsValid(string word, string guess, string score)
  {
    for (int i = 0; i < word.Length; i++)
    {
      if (score[i] == 'G' && word[i] != guess[i])
        return false;

      if (score[i] == 'Y' && !word.Contains(guess[i]))
        return false;
    }
    return true;

  }
}