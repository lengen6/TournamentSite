﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TieRenTournament.Models;

namespace TieRenTournament.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<TieRenTournament.Models.Match> Match { get; set; }
        public DbSet<TieRenTournament.Models.Competitor> Competitor { get; set; }
        public DbSet<TieRenTournament.Models.Initial> Initial { get; set; }
        public DbSet<TieRenTournament.Models.Winner> Winner { get; set; }
        public DbSet<TieRenTournament.Models.Loser> Loser { get; set; }
        public DbSet<TieRenTournament.Models.Eliminated> Eliminated { get; set; }

    }
}