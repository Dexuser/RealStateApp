using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Infrastructure.Identity.Entities;

namespace RealStateApp.Infrastructure.Identity.EntityConfigurations;

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(a =>  a.FirstName).IsRequired().HasMaxLength(200);
        builder.Property(a =>  a.LastName).IsRequired().HasMaxLength(200);
        builder.Property(a => a.IdentityCardNumber).IsRequired(false).HasMaxLength(12);
        builder.Property(a => a.ProfileImagePath).IsRequired(false).HasMaxLength(500);
        builder.Property(a => a.RegisteredAt).IsRequired();
        builder.HasIndex(a => a.IdentityCardNumber).IsUnique();
    }
}