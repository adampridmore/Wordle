namespace Wordle;

using Wordle.Models;

public class PlayerTest{
  [Xunit.Fact]
  public async Task PlayGame(){

    var game = Game.NewGame("GREEN");
    var wordGuesser = new WordGuesser();

    var player = new Player(game, wordGuesser);

    var solution = await player.SolveGame();

    Assert.That(solution.Item1, Is.EqualTo("green"));
  }

  [Xunit.Fact]
  public async Task Fails_to_solve_word_again(){
    var game = Game.NewGame("again");
    var wordGuesser = new WordGuesser();

    var player = new Player(game, wordGuesser);

    var solution = await player.SolveGame();

    Assert.That(solution.Item1, Is.EqualTo("again"));
  }


  [Xunit.Fact]
  public async Task Fails_to_solve_word_other(){
    var game = Game.NewGame("other");
    var wordGuesser = new WordGuesser();

    var player = new Player(game, wordGuesser, true);

    var solution = await player.SolveGame();

    Assert.That(solution.Item1, Is.EqualTo("other"));
  }
  
}
