using System.ComponentModel.DataAnnotations;

public static class GameStates
{
      public const string INPROGRESS = "INPROGRESS";
      public const string WON = "WON";
      public const string LOST = "LOST";
}

public class Game
{
      [Required]
      public Guid Id { get; set; }
      
      [Required]
      public Team? Team { get; set; }

      [Required]
      public string Word { get; set; } = "";
      
      [Required]
      public string State { get; set; } = "";  //TODO: Use enum
      public string? Guess1 { get; set; }
      public string? Guess2 { get; set; }
      public string? Guess3 { get; set; }
      public string? Guess4 { get; set; }
      public string? Guess5 { get; set; }
      public string? Guess6 { get; set; }

}