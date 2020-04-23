using Microsoft.EntityFrameworkCore;
using ChessApp.Domain.Models;
using ChessApp.DAL.Configurations;

namespace ChessApp.DAL
{
    public class ChessAppDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }

        public ChessAppDbContext(DbContextOptions<ChessAppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new GameConfiguration());
            builder.ApplyConfiguration(new PlayerConfiguration());
        }
    }
}