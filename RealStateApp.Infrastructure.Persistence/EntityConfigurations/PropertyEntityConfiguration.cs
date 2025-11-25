using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property = RealStateApp.Core.Domain.Entities.Property;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("Properties");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(9);

        builder.HasIndex(p => p.Code).IsUnique();

        builder.Property(p => p.Price).HasColumnType("decimal(17,2)").IsRequired();
        builder.Property(p => p.SizeInMeters).IsRequired();
        builder.Property(p => p.Rooms).IsRequired();
        builder.Property(p => p.Bathrooms).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1999);
        builder.Property(p => p.IsAvailable).HasDefaultValue(true);
        builder.Property(p => p.AgentId).HasMaxLength(450).IsRequired();

        builder.HasOne(p => p.PropertyType)
            .WithMany(pt => pt.Properties)
            .HasForeignKey(p => p.PropertyTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.SaleType)
            .WithMany(st => st.Properties)
            .HasForeignKey(p => p.SaleTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // La relación con propertyImages está configurada desde propertyImages

        builder.HasMany(p => p.PropertyImprovements)
            .WithOne(pi => pi.Property)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        // La relación con Offers está configurada desde Offers

        // La relación con ChatsMessages esta configurada en ChatMessages.
       
        // La relación con FavoriteProperties está configurada desde FavoriteProperties
;
    }
}