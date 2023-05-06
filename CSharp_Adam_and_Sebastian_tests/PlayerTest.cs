namespace Wordle;

using Wordle.Models;

public class PlayerTest{
  [Xunit.Fact]
  public void PlayGame(){

    var game = Game.NewGame("GREEN");
    var wordGuesser = new WordGuesser();

    var player = new Player(game, wordGuesser);

    var solution = player.SolveGame();

    Assert.That(solution.Item1, Is.EqualTo("green"));
  }

  [Xunit.Fact]
  public void Fails_to_solve_word_again(){
    var game = Game.NewGame("again");
    var wordGuesser = new WordGuesser();

    var player = new Player(game, wordGuesser);

    var solution = player.SolveGame();

    Assert.That(solution.Item1, Is.EqualTo("again"));
  }
}
