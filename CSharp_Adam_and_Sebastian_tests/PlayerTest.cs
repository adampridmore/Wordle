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
  public void Fails_to_solve_word_again(){
    var game = Game.NewGame("again");
    var wordGuesser = new WordGuesser();

    var player = new Player(game, wordGuesser);

    var solution = player.SolveGame();

    Assert.That(solution.Item1, Is.EqualTo("again"));
  }

  
  // [Xunit.Fact]
  // public void Score_all_words(){
  //   var guess = "first";
  //   var wordGuesser = new WordGuesser();
  //   wordGuesser.Words
  //     .Select(word =>(word, WordGuesser.ScoreGuessAgainstWord(guess, word)))
  //     .Where(wordGuess => new List<string>{"again", "which", "first"}.Contains(wordGuess.word))
  //     .ToList().ForEach(wordSore => {
  //       var (word, score) = wordSore;
  //       System.Console.WriteLine($"{word} {score}");
  //     })
  //     ;
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
