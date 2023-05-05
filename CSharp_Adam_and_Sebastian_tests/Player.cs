namespace CSharp_Adam_and_Sebastian_tests;

public class Player{

  public Game Game {get;}
  private readonly WordGuesser _wordGuesser;
  
  public Player(Game game, WordGuesser wordGuesser){
    Game = game;
    _wordGuesser = wordGuesser;
  }
  
  public (string, Game) SolveGame(){
    var game = Game;
    
    var guess = "first";
 
    var wordGuesser = new WordGuesser();
    
    while (game.State == GameState.InProgress){  
      
      game = game.MakeGuess(guess);
      // System.Console.WriteLine($"Guess: [{guess}] score: [{game.LastGuessScore}]");
      if (game.State == GameState.Won) continue;
      
      guess = wordGuesser.GetNextGuess(guess, game.LastGuessScore, game.Guesses);
      // System.Console.WriteLine($"NextGuess: [{guess}]");
      
      // System.Threading.Thread.Sleep(System.TimeSpan.FromMilliseconds(100));
      
      // System.Console.WriteLine($"Guess: {guess} Score:{game.LastGuessScore} State: {game.State}");
    }

    return (guess, game);
  }
}
