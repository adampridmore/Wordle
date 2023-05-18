namespace Wordle;

using Wordle.Models;
using Wordle.Models2;
using System.Collections.Immutable;

public abstract class BaseGame {
  public BaseGame(GameState2 gameState2){
    GameState2 = gameState2;
  }
  public GameState2 GameState2 {get;}

  public abstract Task<BaseGame> MakeGuess(string guessWord);
}

public class Game : BaseGame {
  private string Word {get;}
  public static Game NewGame(string word){
    return new Game(
      word,
      GameState2.NewGame);
  }

  private Game(string word, GameState2 gameState) : base(gameState){
    Word = word;
  }

  public override Task<BaseGame> MakeGuess(string guessWord){
    var guessScore = WordGuesser.ScoreGuessAgainstWord(guessWord, Word);

    var invalidCharactersArray = WordGuesser.GetInvalidLetters(guessWord, guessScore).ToArray();
    var invalidCharacters = ImmutableHashSet.Create(invalidCharactersArray.ToArray());
    

    var state = GameState2.NextGameState(new GuessAndScore(new Guess(guessWord), new GuessScore(guessScore)), invalidCharacters);

    return Task.FromResult((BaseGame)new Game(Word, state));
  }
}
