namespace CSharp_Adam_and_Sebastian_tests;

public class Player{

  public Game Game {get;}
  private readonly WordGuesser _wordGuesser;
  private readonly bool _logging;

  private void WriteLine(string message) {
    if (_logging) {
      System.Console.WriteLine(message);
    }
  }
  
  public Player(Game game, WordGuesser wordGuesser, bool logging = false){
    Game = game;
    _wordGuesser = wordGuesser;
    _logging = logging;
  }
  
  public (string, Game) SolveGame(){
    var game = Game;
    
    var guess = "first";
 
    var wordGuesser = new WordGuesser();
    
    while (game.State == GameState.InProgress){  
      
      game = game.MakeGuess(guess);
      
      WriteLine($"Guess: [{guess}] score: [{game.LastGuessScore}]");
      if (game.State == GameState.Won) continue;
      
      guess = wordGuesser.GetNextGuess(guess, game.LastGuessScore, game.Guesses);
      WriteLine($"NextGuess: [{guess}]");
      
      if (_logging) {
        System.Threading.Thread.Sleep(System.TimeSpan.FromMilliseconds(100));
      }
      
      WriteLine($"Guess: [{guess}] Score:{game.LastGuessScore} State: {game.State}");
    }

    return (guess, game);
  }
}
