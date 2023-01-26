public class Words
{
  private HashSet<string> words;
  private Random random = new Random();

  public Words()
  {
    words = File.ReadAllLines("wwwroot/words.txt")
                .Select(word => word.ToUpper())
                .ToHashSet();
  }

  public bool WordExists(string word)
  {
    return words.Contains(word);
  }

  public string RandomWord()
  {
    var randomWordNumber = random.Next(0, words.Count - 1);
    return words.ElementAt(randomWordNumber);
  }
}