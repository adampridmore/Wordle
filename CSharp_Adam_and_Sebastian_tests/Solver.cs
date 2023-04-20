using System.Collections;
using System;

public class Solver {
  private readonly List<String> _words;

  public Solver(List<String> words) {
    _words = words;
  }

  public Solver(){
    _words = File.ReadAllLines("words.txt").ToList();
  }

  public String GetNextGuess(String lastGuess, String lastResult) {
    var filteredWords = _words.Where(word => Solver.ScoreGuessAgainstWord(lastGuess, word) == lastResult);

    return filteredWords.First();
  }

  public static String ScoreGuessAgainstWord(string guess, string word) {
    var guessLowerCase = guess.ToLowerInvariant();
    var wordLowerCase = word.ToLowerInvariant();
    
    var result = new char[5];
    for(int i = 0 ; i < 5 ; i++){
      if (guessLowerCase[i] == wordLowerCase[i]){
        result[i] = 'G';
      } else if (wordLowerCase.ToArray().Contains(guessLowerCase[i])) {
        result[i] = 'Y';
      } else {
        result[i] = ' ';
      }
    }

    return new String(result);
  }
}
