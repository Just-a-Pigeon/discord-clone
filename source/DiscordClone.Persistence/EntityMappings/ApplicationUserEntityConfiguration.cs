using DiscordClone.Domain.Entities.Consultation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiscordClone.Persistence.EntityMappings;

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasMany(au => au.Friends).WithOne(x => x.User).HasForeignKey(x => x.UserId);
        builder.HasMany(au => au.FriendOf).WithOne(x => x.Friend).HasForeignKey(x => x.FriendId);
        //builder.HasMany(au => au.ServerMembers).WithOne(x => x.User).HasForeignKey(x => x.UserId);
    }
}