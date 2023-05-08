namespace Wordle;

using Wordle.Models;

public class WebGame : IGame
{
  // Real team
  // var teamId = new System.Guid("d24641c0-0fec-457b-b257-19344790aad9"); // SeeSharpers
  
  // local team
  private Guid teamId = new System.Guid("066994ae-0a1a-4e6c-90a4-0a478e5c8155"); // SeeSharpers

  private const string localBaseUrl = "http://localhost:5130";
  private Api Api = new Api(localBaseUrl);

  public GameState State {get; private set;}

  public string LastGuessScore { get; private set; }

  public string InvalidCharacters {get;set;}

  public String[] Guesses {get; private set; }

  public int GuessCount { get; set;}

  private Guid _gameId;
  private WordGuesser _wordGuesser;

  public async Task<IGame> MakeGuess(string guessWord){

    string nextGuess;
    if (GuessCount == 0){
      _gameId = await Api.StartNewGame(teamId);
      Console.WriteLine("GameId: " + _gameId);
      nextGuess = "first";
    } else {
      nextGuess = _wordGuesser.GetNextGuess(Guesses.Last(),LastGuessScore, Guesses, InvalidCharacters);
    }

    NewGuessResponse newGuessResponse = await Api.GuessWord(_gameId,nextGuess);
    InvalidCharacters = InvalidCharacters + WordGuesser.GetInvalidLetters(nextGuess, LastGuessScore);
    LastGuessScore = newGuessResponse.Score;
    State = newGuessResponse.State;
    Guesses = Guesses.ToList().Append(nextGuess).ToArray();
    GuessCount++;

    return this;
  }
  
  public WebGame(WordGuesser wordGuesser){
    InvalidCharacters = "";
    LastGuessScore = "";
    _wordGuesser = wordGuesser;
    Guesses = new List<string>().ToArray();
    GuessCount = 0;
  }
}

public static class WebSolver{

  
  
  public static async Task Run(){
      
    var wordGuesser = new WordGuesser();

    var webGame = new WebGame(wordGuesser);

    var player = new Player(webGame, wordGuesser);
    
    (string, IGame) solution = await player.SolveGame();

    var message = $"Answer: {solution.Item1} guess count: {solution.Item2.GuessCount} guesses: {String.Join(",",solution.Item2.Guesses)}";


    System.Console.WriteLine(message);
  }
}
