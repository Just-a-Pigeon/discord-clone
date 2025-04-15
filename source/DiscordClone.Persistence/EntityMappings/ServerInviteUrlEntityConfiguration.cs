using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Persistence.EntityMappings;

public class ServerInviteUrlEntityConfiguration : IEntityTypeConfiguration<ServerInviteUrl>
{
    public void Configure(EntityTypeBuilder<ServerInviteUrl> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.UriParameter).HasMaxLength(20);

        builder.HasIndex(x => x.UriParameter);
    }
}