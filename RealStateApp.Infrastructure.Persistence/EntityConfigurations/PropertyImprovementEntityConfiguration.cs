using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class PropertyImprovementEntityConfiguration : IEntityTypeConfiguration<PropertyImprovement>
{
    public void Configure(EntityTypeBuilder<PropertyImprovement> builder)
    {
        builder.ToTable("PropertyImprovements");

        builder.HasKey(pi => pi.Id);
        builder.HasIndex(pi => new { pi.PropertyId, pi.ImprovementId });

        builder.HasOne(pi => pi.Property)
            .WithMany(p => p.PropertyImprovements)
            .HasForeignKey(pi => pi.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pi => pi.Improvement)
            .WithMany(i => i.PropertyImprovements)
            .HasForeignKey(pi => pi.ImprovementId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}