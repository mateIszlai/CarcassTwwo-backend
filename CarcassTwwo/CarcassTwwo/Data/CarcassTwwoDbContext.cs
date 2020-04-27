using CarcassTwwo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarcassTwwo.Data
{
    public class CarcassTwwoDbContext : DbContext
    {
        public CarcassTwwoDbContext(DbContextOptions<CarcassTwwoDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<Carcassonne> Games { get; set; }
        //public DbSet<Client> Clients { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<LandType> LandTypes { get; set; }
        public DbSet<Card> Cards { get; set; }
    }
}
