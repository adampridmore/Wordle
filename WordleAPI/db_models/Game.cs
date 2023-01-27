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
  public DateTime DateStarted { get; set; }

  [Required]
  [MaxLength(5)]
  public string Word { get; set; } = string.Empty;

  [Required]
  public GameState State { get; set; } = GameState.InProgress;

  [MaxLength(5)]
  public string? Guess1 { get; set; }

  [MaxLength(5)]
  public string? Guess2 { get; set; }

  [MaxLength(5)]
  public string? Guess3 { get; set; }

  [MaxLength(5)]
  public string? Guess4 { get; set; }

  [MaxLength(5)]
  public string? Guess5 { get; set; }

  [MaxLength(5)]
  public string? Guess6 { get; set; }
}