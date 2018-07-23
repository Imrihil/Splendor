using Splendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splendor.DAL
{
    public class SplendorDbContext : DbContext
    {
        public SplendorDbContext() : base("DefaultConnection") { }

        public DbSet<Aristocrat> Aristocrats { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<Gem> Gems { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<PlayerInGame> PlayerInGames { get; set; }

        public DbSet<AristocratInGame> AristocratInGames { get; set; }

        public DbSet<CardInGame> CardInGames { get; set; }

        public DbSet<Log> Logs { get; set; }
    }
}
