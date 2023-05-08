
using Wordle.Models;
using Wordle;

public static class Program
{
  static async Task Main()
  // static void Main()
  {
    // await WebSolver.Run();

    await PlayAllGames.Run();

    // await ConsolePlayer.Run();
  }
}
