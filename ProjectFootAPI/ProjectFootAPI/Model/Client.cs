using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectFootAPI.Model;

public class Client
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(50)]
    public string Name { get; set; }
    [Required, MaxLength(50)]
    public string Password { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public int Points { get; set; } = 500;
    public bool isBlocked { get; set; }
    public bool isAdmin { get; set; }
    public ICollection<Club>? FavoriteClubs { get; set; } = new List<Club>();
    public ICollection<Ligue>? FavoriteLigues { get; set; } = new List<Ligue>();
    public ICollection<Bet>? Bets { get; set; } = new List<Bet>();
}
