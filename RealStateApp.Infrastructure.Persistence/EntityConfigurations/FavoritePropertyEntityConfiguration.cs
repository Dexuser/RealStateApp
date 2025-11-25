using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class FavoritePropertyEntityConfiguration : IEntityTypeConfiguration<FavoriteProperty>
{
    public void Configure(EntityTypeBuilder<FavoriteProperty> builder)
    {
        builder.ToTable("FavoriteProperties");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.UserId).HasMaxLength(450).IsRequired();
        builder.Property(f => f.PropertyId).IsRequired();
        
        builder.HasOne(f => f.Property)
            .WithMany(p => p.FavoriteProperties)
            .HasForeignKey(f => f.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}