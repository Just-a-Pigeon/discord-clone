using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Persistence.EntityMappings;

public class ServerNodeEntityConfiguration : IEntityTypeConfiguration<ServerNode>
{
    public void Configure(EntityTypeBuilder<ServerNode> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(25).IsRequired();
        builder.Property(x => x.Type).HasColumnType("text").HasMaxLength(50).IsRequired();
        builder.Property(x => x.ChannelTopic).HasMaxLength(500).IsRequired();
        builder.HasMany(x => x.Children).WithOne(x => x.Parent);
    }
}