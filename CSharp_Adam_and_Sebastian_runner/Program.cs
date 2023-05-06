
using Wordle.Models;
using Wordle;

public static class Program
{
  static async Task Main()
  {
    await WebSolver.Run();
  }
}
