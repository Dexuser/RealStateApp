using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Infrastructure.Persistence.EntityConfigurations;

public class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Message).IsRequired().HasMaxLength(2000);
        builder.Property(c => c.SentAt).IsRequired();
        builder.Property(c => c.PropertyId).IsRequired();
        builder.Property(c => c.ReceiverId).HasMaxLength(450).IsRequired();
        builder.Property(c => c.SenderId).HasMaxLength(450).IsRequired();

        builder.HasOne(c => c.Property)
            .WithMany(p => p.ChatMessages)
            .HasForeignKey(c => c.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
   }
}