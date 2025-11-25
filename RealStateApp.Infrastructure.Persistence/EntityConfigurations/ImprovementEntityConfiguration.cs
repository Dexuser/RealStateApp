using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class ImprovementEntityConfiguration : IEntityTypeConfiguration<Improvement>
{
    public void Configure(EntityTypeBuilder<Improvement> builder)
    {
        builder.ToTable("Improvements");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name).IsRequired().HasMaxLength(150);
        builder.Property(i => i.Description).HasMaxLength(1000);
    }
}