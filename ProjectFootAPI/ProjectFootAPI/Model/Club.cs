using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProjectFootAPI.Model;

[Index(nameof(Ranking), nameof(LigueId), IsUnique = true)]
public class Club
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required]
    public string Logo { get; set; }
    [Required]
    public int Ranking { get; set; }
    public int LigueId { get; set; }
    [ForeignKey("LigueId")]
    public Ligue? Ligue { get; set; }
    public ICollection<Client>? Favorites { get; set; } = new List<Client>();
    [InverseProperty("Club1")]
    public ICollection<Match>? MatchesClub1 { get; set; } = new List<Match>();
    [InverseProperty("Club2")]
    public ICollection<Match>? MatchesClub2 { get; set; } = new List<Match>();
}
