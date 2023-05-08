namespace Wordle;

using System.Collections;
using System;

public class WordGuesser {
  
  public List<String> Words {get;}

  public WordGuesser(List<String> words) {
    Words = words;
  }

  public WordGuesser(){
    Words = File.ReadAllLines("words.txt").ToList();
  }

  public String GetNextGuess(String lastGuessWord, String lastResultScore, String[] previousGuesses, string invalidLetters) {
    var filteredWords = 
      Words.Where(word => {
        return
            !WordGuesser.WordContainsAnyLetters(word, invalidLetters)
            &&
            WordGuesser.ScoreGuessAgainstWord(lastGuessWord, word) == lastResultScore
            &&
            (word != lastGuessWord)
            &&
            (!previousGuesses.Contains(word))
            ;
      });

    var nextGuess = filteredWords.FirstOrDefault();

    if (nextGuess == lastGuessWord) {
      throw new Exception($"Last guess [{lastGuessWord}] equals next guess [{nextGuess}]");
    }

    if (nextGuess == null){
      throw new Exception($"No nextGuess for lastGuessWord: [{lastGuessWord}]");
    }

    return nextGuess;
  }


  // TODO: fix scoring
  // Need to have two flavours. One to match the API implementation and one to match the real implementation
  // regarding the scoring of 
  // word             first
  // Guess            aaiit
  // API Score          YY    Counts both i's as a Y
  // Real score         Y     Counts first i as a Y
  public static String ScoreGuessAgainstWord(string guess, string word) {
    var guessLowerCase = guess.ToLowerInvariant().ToArray();
    var wordLowerCase = word.ToLowerInvariant().ToArray();
    
    var result = new char[5];
    for(int i = 0 ; i < 5 ; i++){
      // System.Console.WriteLine($"wordLowerCase: [{new String(wordLowerCase)}]");
      if (guessLowerCase[i] == wordLowerCase[i]){
        result[i] = 'G';
      } else if (wordLowerCase.Contains(guessLowerCase[i])) {
        int matchIndex = Array.IndexOf(wordLowerCase, guessLowerCase[i]);
        wordLowerCase[matchIndex] = '.';
        result[i] = 'Y';
      } else {
        result[i] = ' ';
      }
    }

    return new String(result);
  }

  public static bool WordContainsAnyLetters(string word, string letters)
  {
    return 
      word.ToArray()
        .Where(wordLetter => letters.Contains(wordLetter))
        .Any();
  }

  public static string GetInvalidLetters(string guess, string score)
  {
    var invalidLetters =
      guess.ToArray()
        .Select( (letter, index) => (letter, index) )
        .Where( (letter, index) => score[index] == ' ')
        .Select ( x => x.letter )
        ;
    return new String(invalidLetters.ToArray());
  }
}
