using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class SaleTypeEntityConfiguration : IEntityTypeConfiguration<SaleType>
{
    public void Configure(EntityTypeBuilder<SaleType> builder)
    {
        builder.ToTable("SaleTypes");
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Name).IsRequired().HasMaxLength(100);
        builder.Property(st => st.Description).HasMaxLength(1000);
    }
}