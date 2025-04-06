using DiscordClone.Domain.Entities.Consultation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Persistence.EntityMappings;

public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Content)
            .HasColumnType("NVARCHAR")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);
    }
}