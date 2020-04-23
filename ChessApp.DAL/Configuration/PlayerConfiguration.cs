using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ChessApp.Domain.Models;

namespace ChessApp.DAL.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            const int maxLength = 50;

            builder.HasKey(p => p.Id);

            builder.Property(c => c.Id)
                   .UseIdentityColumn();

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(maxLength);

            builder.ToTable("Players");
        }
    }
}