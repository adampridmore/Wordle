using Xunit;

public class ScoreTests
{
  // https://wordfinder.yourdictionary.com/blog/can-letters-repeat-in-wordle-a-closer-look-at-the-rules/
  [Theory]
  [InlineData("ELUDE","CRANE","    G")]
  [InlineData("ELUDE","HOIST","     ")]
  [InlineData("ELUDE","BULKY"," YY  ")]
  [InlineData("ELUDE","LEDGE","YYY G")]
  [InlineData("ELUDE","ELUDE","GGGGG")]
  public void GuessesAreCorrectlyScored(string actualWord, string guess, string expectedScore)
  {
    var scorer = new Scorer();
    var actualScore = scorer.ScoreGuess(actualWord, guess);
    Assert.Equal(expectedScore, actualScore);
  }

}