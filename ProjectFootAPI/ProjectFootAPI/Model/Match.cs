using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectFootAPI.Model;

public class Match
{
    [Key]
    public int Id { get; set; }
    [Required]
    public int Score1 { get; set; }
    [Required]
    public int Score2 { get; set; }
    public DateTime MatchDateTime { get; set; }
    public bool isTopMatch { get; set; }
    public bool isCanceled { get; set; }
    public int ClubId1 { get; set; }
    [ForeignKey("ClubId1")]
    [InverseProperty("MatchesClub1")]
    public Club Club1 { get; set; }
    public int ClubId2 { get; set; }
    [ForeignKey("ClubId2")]
    [InverseProperty("MatchesClub2")]
    public Club Club2 { get; set; }
    public ICollection<Bet>? Bets { get; set; } = new List<Bet>();

}
