namespace CSharp_Adam_and_Sebastian_tests;

public class GameTests
{
  [Xunit.Fact]
  public void new_game(){
    var game = Game.NewGame("GREEN");

    Assert.That(game.State, Is.EqualTo(GameState.InProgress));
    Assert.That(game.LastGuessScore, Is.EqualTo(""));

    Assert.That(game.GuessCount, Is.EqualTo(0));
  }

  [Xunit.Fact]
  public void new_game_with_one_winning_guess()
  {
    var game = Game.NewGame("GREEN");

    var nextGuessGame = game.MakeGuess("GREEN");
    
    Assert.That(nextGuessGame.State, Is.EqualTo(GameState.Won));
    Assert.That(nextGuessGame.LastGuessScore, Is.EqualTo("GGGGG"));

    Assert.That(nextGuessGame.GuessCount, Is.EqualTo(1));
  }

  [Xunit.Fact]
  public void incorrect_guess()
  {
    var game = Game.NewGame("GREEN");

    var nextGuessGame = game.MakeGuess("PAPER");
    
    Assert.That(nextGuessGame.State, Is.EqualTo(GameState.InProgress));
    Assert.That(nextGuessGame.LastGuessScore, Is.EqualTo("   GY"));

    Assert.That(nextGuessGame.GuessCount, Is.EqualTo(1));
  }
}
