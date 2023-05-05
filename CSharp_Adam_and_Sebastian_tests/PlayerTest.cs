namespace CSharp_Adam_and_Sebastian_tests;

public class PlayerTest{
  // [Fact]
  // public void PlayGame(){

  //   var game = Game.NewGame("GREEN");
  //   var wordGuesser = new WordGuesser();

  //   var player = new Player(game, wordGuesser);

  //   var solution = player.SolveGame();

  //   Assert.That(solution.Item1, Is.EqualTo("green"));
  // }


  [Xunit.Fact]
  public void ConsoleTest(){
    System.Threading.Thread.Sleep(1000);
    System.Console.WriteLine("Test 1 : Start");
    TestContext.Out.WriteLine("A-Test 1 : Start");
    System.Threading.Thread.Sleep(1000);

    for(int i = 0 ; i < 10 ; i++){
      System.Console.WriteLine($"Test 1 - {i}");
      TestContext.Out.WriteLine($"A-Test 1 - {i}");
      System.Threading.Thread.Sleep(1000);
    }
  }

  // [Fact]
  // public void Fails_to_solve_word_again(){
  //   var game = Game.NewGame("again");
  //   var wordGuesser = new WordGuesser();

  //   var player = new Player(game, wordGuesser);

  //   var solution = player.SolveGame();

  //   Assert.That(solution.Item1, Is.EqualTo("again"));
  // }

  // [Fact]
  // public void PlayAllGames(){

  //   var wordGuesser = new WordGuesser();

  //   for(int i = 0 ; i < 21; i++){
  //   // foreach(String word in wordGuesser.Words){
  //     var stopWatch = System.Diagnostics.Stopwatch.StartNew();

  //     var word = wordGuesser.Words[i];

  //     System.Console.WriteLine($"Word to play: {word}");
    
  //     var game = Game.NewGame(word);

  //     var player = new Player(game, wordGuesser);

  //     var solution = player.SolveGame();

  //     System.Console.WriteLine($"Word: {word} guesses: {solution.Item2.GuessCount} {stopWatch.ElapsedMilliseconds}ms");
  //   }
  // }
}
