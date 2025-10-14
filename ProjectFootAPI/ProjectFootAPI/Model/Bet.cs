using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFootAPI.Model;

public class Bet
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int Point { get; set; }
    public DateTime BetTiming { get; set; }
    public bool isVictory { get; set; }
    public int ClientId { get; set; }
    [ForeignKey("ClientId")]
    public Client? Client { get; set; }
    public int MatchId { get; set; }
    [ForeignKey("MatchId")]
    public Match? Match { get; set; }
}
