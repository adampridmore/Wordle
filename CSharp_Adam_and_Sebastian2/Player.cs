// namespace Wordle;

// using Wordle.Models;



// public class Player{

//   public IGame Game {get;}
//   private readonly WordGuesser _wordGuesser;
//   private readonly bool _logging;

//   private void WriteLine(string message) {
//     if (_logging) {
//       System.Console.WriteLine(message);
//     }
//   }
  
//   public Player(IGame game, WordGuesser wordGuesser,  bool logging = false){
//     Game = game;
//     _wordGuesser = wordGuesser;
//     _logging = logging;
//   }
  
//   public async Task<(string, IGame)> SolveGame(){
//     var game = Game;
    
//     var guess = "first";
 
//     var wordGuesser = new WordGuesser();
    
//     while (game.State == GameState.InProgress){  
      
//       game = await game.MakeGuess(guess);
     
//       WriteLine($"Guess: [{guess}] score: [{game.LastGuessScore}] InvalidChars: {game.InvalidCharacters}");
//       if (game.State == GameState.Won) continue;
      
//       guess = wordGuesser.GetNextGuess(guess, game.LastGuessScore, game.Guesses, game.InvalidCharacters);
      
//       WriteLine($"NextGuess: [{guess}]");
      
//       if (_logging) {
//         System.Threading.Thread.Sleep(System.TimeSpan.FromMilliseconds(100));
//       }
      
//       // WriteLine($"Guess: [{guess}] Score:{game.LastGuessScore} State: {game.State}");
//     }

//     (string, IGame) result = (guess, game);
//     return result;
//   }
// }
