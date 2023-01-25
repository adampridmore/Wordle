using System.ComponentModel.DataAnnotations;

public class Team
{
    public Guid Id { get; set; }
    
    [Required, MinLength(1), MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}