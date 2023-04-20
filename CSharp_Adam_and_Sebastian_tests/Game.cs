namespace CSharp_Adam_and_Sebastian_tests;

public enum GameState
{
  InProgress,
  Won,
  Lost
}

public class Game {

  private const string WinScore = "GGGGG";
  public GameState State { get; }

  public string LastGuessScore { get; }

  public int GuessCount { get; }

  private readonly string _word;

  public static Game NewGame(string word){
    return new Game(word, 0, "", GameState.InProgress);
  }

  private Game(string word, int guessCount, string lastGuessScore, GameState state){
    _word = word;
    GuessCount = guessCount;
    LastGuessScore = lastGuessScore;
    State = state;
  }

  public Game MakeGuess(string guessWord){    
    var lastGuessScore = Solver.ScoreGuessAgainstWord(guessWord, _word);

    var nextGameState = lastGuessScore == WinScore?GameState.Won:GameState.InProgress;

    return new Game(_word, GuessCount+1, lastGuessScore, nextGameState);
  }
}
