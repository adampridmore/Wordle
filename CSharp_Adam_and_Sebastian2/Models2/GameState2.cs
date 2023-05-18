namespace Wordle.Models2;

// using System.Collections.Generic;
using System.Collections.Immutable;

public record Guess(string word);
public record GuessScore(string score){
  public static GuessScore WinScore = new GuessScore("GGGGG");
  
  public Boolean Won { 
    get {
      return this == WinScore;
    } 
  }
};

public record GuessAndScore(Guess guess, GuessScore score);

public enum GameStatus{
  InProgress,
  Lost,
  Won
}

public record GameState2(
  ImmutableList<GuessAndScore> PreviousGuesses,
  ImmutableHashSet<Char> InvalidLetters){

  public static GameState2 NewGame {
    get {
      return new GameState2(ImmutableList<GuessAndScore>.Empty, ImmutableHashSet<Char>.Empty);
    }
  }

  public GameStatus Status {
    get {
      var lastGuess = PreviousGuesses.LastOrDefault(); 
      // TODO: Replace with switch expression
      if (lastGuess == null){
        return GameStatus.InProgress;
      }
      if (lastGuess.score.Won){
        return GameStatus.Won;
      }
      return GameStatus.InProgress;
      
      // switch {
      //   null => GameStatus.InProgress,
      //   {GuessAndScore: var x} => GameStatus.Won,
      //   // _  when !_.Won => GameStatus.InProgress
      //   // { Age: var age } when age < 18 => "minor person",
      // };
    }
  }

  public GameState2 NextGameState(GuessAndScore guessAndScore, ImmutableHashSet<Char> invalidLetters){
    var newPreviousGuesses = this.PreviousGuesses.Add(guessAndScore);
    var newInvalidLetters = InvalidLetters.Union(invalidLetters);
    
    return new GameState2(
      newPreviousGuesses,
      newInvalidLetters);
  }
}
