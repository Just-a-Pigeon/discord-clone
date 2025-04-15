using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Persistence.EntityMappings;

public class ServerEntityConfiguration : IEntityTypeConfiguration<Server>
{
    public void Configure(EntityTypeBuilder<Server> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(25).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.ImagePath).HasMaxLength(100);
        builder.Property(x => x.BannerImagePath).HasMaxLength(100);
    }
}