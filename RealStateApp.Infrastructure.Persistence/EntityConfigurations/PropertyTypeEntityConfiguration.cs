using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;
using Property = Microsoft.EntityFrameworkCore.Metadata.Internal.Property;

namespace RealStateApp.Infrastructure.Persistence.Contexts;

public class PropertyTypeEntityConfiguration : IEntityTypeConfiguration<PropertyType>
{
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.ToTable("PropertyTypes");
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Name).IsRequired().HasMaxLength(100);
        builder.Property(pt => pt.Description).HasMaxLength(1000);
    }
}