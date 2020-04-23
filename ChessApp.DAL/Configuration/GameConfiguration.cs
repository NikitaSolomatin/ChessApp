using ChessApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChessApp.DAL.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            const int maxLength = 50;

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                   .UseIdentityColumn();

            builder.Property(c => c.Result)
                   .IsRequired()
                   .HasMaxLength(maxLength);

            builder.HasOne(c => c.Player)
                   .WithMany(p => p.Games)
                   .HasForeignKey(c => c.PlayerId);

            builder.ToTable("Games");
        }
    }
}