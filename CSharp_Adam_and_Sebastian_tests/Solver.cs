
using System.Collections;
using System;

public class Solver {
    private readonly List<String> _words;
    
    public Solver(List<String> words) {
      _words = words;
    }

    public String GetNextGuess(String lastGuess, String lastResult) {
      var filteredWords = _words.Where(word => Solver.ScoreGuessAgainstWord(lastGuess, word) == lastResult);

      return filteredWords.First();
    }

    public static String ScoreGuessAgainstWord(string guess, string word) {
      var result = new char[5];
      for(int i = 0 ; i < 5 ; i++){
          if (guess[i] == word[i]){
              result[i] = 'G';
          } else {
              result[i] = ' ';
          }
      }

      for(int i = 0 ; i < 5 ; i++){
          if (result[i] != 'G'){
              if (word.ToArray().Contains(guess[i]))
              {
                  result[i] = 'Y';
              }
          }
      }

      return new String(result);
  }
}
