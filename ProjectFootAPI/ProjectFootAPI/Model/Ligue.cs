using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectFootAPI.Model;

public class Ligue
{
    [Key]
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required]
    public string Logo { get; set; }
    public ICollection<Club>? Clubs { get; set; } = new List<Club>();
    public ICollection<Client>? Favorites { get; set; } = new List<Client>();
}
