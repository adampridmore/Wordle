namespace CSharp_Adam_and_Sebastian_tests;

public class WordGuesserTests
{
  [Test]
  public void NoMatch()
  {
    var guess = "WORDS";
    var actualWord = "ZZZZZ";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("     "));
  }

  [Test]
  public void FullMatch()
  {
    var guess = "WORDS";
    var actualWord = "WORDS";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGGGG"));
  }

  
  [Test]
  public void FullMatch_ignore_case()
  {
    var guess = "Words";
    var actualWord = "WORDS";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGGGG"));
  }

  [Test]
  public void OneYellowMatch()
  {
    var guess = "A     ";
    var actualWord = "BBBBA";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("Y    "));
  }

  [Test]
  public void MixMatches(){
    var guess = "ABBZZ";
    var actualWord = "ABCDE";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGY  "));
  }

  [Test]
  public void GetNextGuess(){
    var solver = new WordGuesser();

    var nextGuess = solver.GetNextGuess("words", "G  GG");

    Assert.That(nextGuess, Is.EqualTo(("winds")));
  }

    // [Test]
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
