using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class PropertyImageEntityConfiguration : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.ToTable("PropertyImages");
        builder.HasKey(pi => pi.Id);
        builder.Property(pi => pi.ImagePath).IsRequired().HasMaxLength(500);
        builder.Property(pi => pi.IsMain).IsRequired().HasDefaultValue(false);
        
        builder.HasOne(pi => pi.Property)
            .WithMany(p => p.PropertyImages)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}