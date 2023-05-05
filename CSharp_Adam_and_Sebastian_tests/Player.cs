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
    
    // System.Console.WriteLine($"1");

    while (game.State == GameState.InProgress){  
      
      // throw new System.Exception("bang!");
      
      // System.Console.WriteLine($"2");
      game = game.MakeGuess(guess);
      // System.Console.WriteLine($"3");
      guess = wordGuesser.GetNextGuess(guess, game.LastGuessScore);
      // System.Console.WriteLine($"3");

      // System.Threading.Thread.Sleep(System.TimeSpan.FromMilliseconds(100));
      // System.Console.WriteLine($"Guess: {guess} Score:{game.LastGuessScore} State: {game.State}");
    }

    return (guess, game);
  }
}
