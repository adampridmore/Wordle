namespace Wordle;

public class WordGuesserTests
{
  [Xunit.Fact]
  public void NoMatch()
  {
    var guess = "WORDS";
    var actualWord = "ZZZZZ";
    var result = Wordle.WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

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
  public void YellowForFirstMatch(){
    var guess = "BBCCC";
    var actualWord = "CXXXX";
    var result = WordGuesser.ScoreGuessAgainstWord(guess, actualWord);

    Assert.That(result, Is.EqualTo("  Y  "));
  }

  [Xunit.Fact]
  public void GetNextGuess(){
    var solver = new WordGuesser();
    var previousGuesses = new String[]{};

    var invalidLetters = "i";

    var nextGuess = solver.GetNextGuess("words", "G  GG", previousGuesses, invalidLetters);

    Assert.That(nextGuess, Is.EqualTo(("weeds")));
  }

  [Xunit.Fact]
  public void WordContainsAnyOfTheseLetters(){
    Assert.That(WordGuesser.WordContainsAnyLetters("apple","a"), Is.True,"a");
    Assert.That(WordGuesser.WordContainsAnyLetters("apple","b"), Is.False,"b");
    Assert.That(WordGuesser.WordContainsAnyLetters("apple","xp"), Is.True, "xp");
    Assert.That(WordGuesser.WordContainsAnyLetters("apple",""), Is.False, "");
  }

  [Xunit.Fact]
  public void GetInvalidLettersFromGuess(){
    Assert.That(WordGuesser.GetInvalidLetters("abcde", "     "), Is.EqualTo("abcde"));
    Assert.That(WordGuesser.GetInvalidLetters("abcde", " GGY "), Is.EqualTo("ae"));
  }

  // TODO: Test if previousGuesses contains word

  // TODO: Don't guess words with letters that have no match

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
