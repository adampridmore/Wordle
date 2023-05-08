using Wordle;
using Wordle.Models;

// TODO: Move common elements to base game with overrides

public class ConsoleGame : IGame{
  private const string WinScore = "GGGGG";
  public GameState State { get; }

  public string LastGuessScore { get; }

  public string InvalidCharacters {get;set;}

  public String[] Guesses {get; }

  public int GuessCount { get; }

  public static IGame NewGame(){
    return new ConsoleGame(0, "", GameState.InProgress, new String[] {});
  }

  private ConsoleGame(int guessCount, string lastGuessScore, GameState state, string [] guesses){
    InvalidCharacters = "";
    GuessCount = guessCount;
    LastGuessScore = lastGuessScore;
    State = state;
    Guesses = guesses;
  }

  public Task<IGame> MakeGuess(string guessWord){
    Console.WriteLine($"My guess is [{guessWord}].");
    Console.WriteLine($"What does it score?");
    var lastGuessScore = Console.ReadLine()??"";
    
    var nextGameState = lastGuessScore == WinScore?GameState.Won:GameState.InProgress;

    var newGuesses = Guesses.Append(guessWord).ToArray();

    // TODO : Something to do with invalid characters
    return Task.FromResult<IGame>(new ConsoleGame(GuessCount+1, lastGuessScore, nextGameState, newGuesses));
  }
}

public class ConsolePlayer{
  public static async Task Run(){
    var wordGuesser = new WordGuesser();

    var consoleGame = ConsoleGame.NewGame();

    var player = new Player(consoleGame, wordGuesser);
    
    (string, IGame) solution = await player.SolveGame();

    var message = $"Answer: {solution.Item1} guess count: {solution.Item2.GuessCount} guesses: {String.Join(",",solution.Item2.Guesses)}";

    System.Console.WriteLine(message);
  }
}
