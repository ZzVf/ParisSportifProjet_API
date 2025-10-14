using System;
using Microsoft.EntityFrameworkCore;
using ProjectFootAPI.Model;

namespace ProjectFootAPI.Data;

public class ProjectFootContext : DbContext
{
    public ProjectFootContext(DbContextOptions<ProjectFootContext> options)
            : base(options)
    {
    }

    // DbSet pour chaque entité du modèle
    public DbSet<Bet> Bets { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Ligue> Ligues { get; set; }
    public DbSet<Match> Matches { get; set; }
}
