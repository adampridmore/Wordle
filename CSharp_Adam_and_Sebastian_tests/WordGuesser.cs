using System.Collections;
using System;

public class WordGuesser {
  private readonly List<String> _words;

  public WordGuesser(List<String> words) {
    _words = words;
  }

  public WordGuesser(){
    _words = File.ReadAllLines("words.txt").ToList();
  }

  public String GetNextGuess(String lastGuess, String lastResult) {
    var filteredWords = _words.Where(word => WordGuesser.ScoreGuessAgainstWord(lastGuess, word) == lastResult);

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
