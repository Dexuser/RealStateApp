using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class OfferEntityConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserId).HasMaxLength(450).IsRequired();
        builder.Property(o => o.Amount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.Status).IsRequired();
        builder.Property(o => o.PropertyId).IsRequired();

        builder.HasOne(o => o.Property)
            .WithMany(p => p.Offers)
            .HasForeignKey(o => o.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}