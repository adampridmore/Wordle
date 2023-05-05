namespace CSharp_Adam_and_Sebastian_tests;

public class WordGuesserTests
{
  [Xunit.Fact]
  public void NoMatch()
  {
    var guess = "WORDS";
    var actualWord = "ZZZZZ";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("     "));
  }

  [Xunit.Fact]
  public void FullMatch()
  {
    var guess = "WORDS";
    var actualWord = "WORDS";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGGGG"));
  }

  
  [Xunit.Fact]
  public void FullMatch_ignore_case()
  {
    var guess = "Words";
    var actualWord = "WORDS";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGGGG"));
  }

  [Xunit.Fact]
  public void OneYellowMatch()
  {
    var guess = "A     ";
    var actualWord = "BBBBA";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("Y    "));
  }

  [Xunit.Fact]
  public void MixMatches(){
    var guess = "ABBZZ";
    var actualWord = "ABCDE";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGY  "));
  }

  [Xunit.Fact]
  public void GetNextGuess(){
    var solver = new WordGuesser();
    var previousGuesses = new String[]{};

    var nextGuess = solver.GetNextGuess("words", "G  GG", previousGuesses);

    Assert.That(nextGuess, Is.EqualTo(("winds")));
  }

  // TODO: Test if previousGuesses contains word

  // [Xunit.Fact]
  // public void Dont_guess_same_word(){

  //   var wordGuesser = new WordGuesser(new List<string>() {"AAAAC", "AAAAD"});
  //   var nextGuess = wordGuesser.GetNextGuess("AAAAB","GGGG ");
  //   System.Console.WriteLine(nextGuess);
  //   Assert.That(nextGuess, Is.EqualTo("AAAAD"));
  // }

    // [Fact]
    // public void FilterWordList() {
    //     var words = File.ReadAllLines("words.txt");

    //     var guess = "words"; // wards
    //     var actualResult = "G  GG";

    //     Console.WriteLine($"First Word: [{words.First()}]");

    //     var filteredWords = words.Where(word => Solver.ScoreGuessAgainstWord(guess, word) == actualResult);

    //     Console.WriteLine($"Filtered {filteredWords.Count()}");
    //     foreach(var filteredWord in filteredWords) {
    //         Console.WriteLine(filteredWord);
    //     }
    // }
}
