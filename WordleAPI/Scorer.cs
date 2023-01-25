public class Scorer
{
  public string ScoreGuess(string actualWord, string guess)
  {
    var score = "";
    for (var i = 0; i < actualWord.Length; i++)
    {
      if (guess[i] == actualWord[i])
      {
        score += "G";
      }
      else if (actualWord.Contains(guess[i]))
      {
        score += "Y";
      }
      else
      {
        score += " ";
      }
    }
    return score;
  }
}