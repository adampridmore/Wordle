using System.ComponentModel.DataAnnotations;

public enum GameState
{
     InProgress,
     Won,
     Lost
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
      public GameState State { get; set; } = GameState.InProgress;
      public string? Guess1 { get; set; }
      public string? Guess2 { get; set; }
      public string? Guess3 { get; set; }
      public string? Guess4 { get; set; }
      public string? Guess5 { get; set; }
      public string? Guess6 { get; set; }

}