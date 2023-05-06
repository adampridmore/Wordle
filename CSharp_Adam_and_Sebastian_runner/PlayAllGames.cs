namespace Wordle;

public class PlayAllGames{
  public static async Task Run(){
    var wordGuesser = new WordGuesser();

    using (var writer = System.IO.File.CreateText("words-solve-count.csv")){
      foreach(String word in wordGuesser.Words){
        var stopWatch = System.Diagnostics.Stopwatch.StartNew();

        System.Console.WriteLine($"Word to play: {word}");
      
        var game = Game.NewGame(word);

        var player = new Player(game, wordGuesser);

        var solution = await player.SolveGame();

        var message = $"Word: {word} guesses: {solution.Item2.GuessCount} {stopWatch.ElapsedMilliseconds}ms";
        
        System.Console.WriteLine(message);
        writer.WriteLine(message);
      }
    }
  }
}
