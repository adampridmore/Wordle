namespace CSharp_Adam_and_Sebastian_tests;

public class Tests
{
  [Test]
  public void NoMatch()
  {
    var guess = "WORDS";
    var actualWord = "ZZZZZ";
    var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("     "));
  }

  [Test]
  public void FullMatch()
  {
    var guess = "WORDS";
    var actualWord = "WORDS";
    var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGGGG"));
  }

  [Test]
  public void OneYellowMatch()
  {
    var guess = "A     ";
    var actualWord = "BBBBA";
    var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("Y    "));
  }

  [Test]
  public void MixMatches(){
    var guess = "ABBZZ";
    var actualWord = "ABCDE";
    var result = Solver.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("GGY  "));
  }

  [Test]
  public void GetNextGuess(){
    var solver = new Solver(File.ReadAllLines("words.txt").ToList());

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
