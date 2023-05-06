namespace Wordle;

using Wordle.Models;

public class Game {

  private const string WinScore = "GGGGG";
  public GameState State { get; }

  public string LastGuessScore { get; }

  public String[] Guesses {get; }

  public int GuessCount { get; }

  private readonly string _word;

  public static Game NewGame(string word){
    return new Game(word, 0, "", GameState.InProgress, new String[] {});
  }

  private Game(string word, int guessCount, string lastGuessScore, GameState state, string [] guesses){
    _word = word;
    GuessCount = guessCount;
    LastGuessScore = lastGuessScore;
    State = state;
    Guesses = guesses;
  }

  public Game MakeGuess(string guessWord){    
    var lastGuessScore = WordGuesser.ScoreGuessAgainstWord(guessWord, _word);

    var nextGameState = lastGuessScore == WinScore?GameState.Won:GameState.InProgress;

    var newGuesses = Guesses.Append(guessWord).ToArray();

    return new Game(_word, GuessCount+1, lastGuessScore, nextGameState, newGuesses);
  }
}
