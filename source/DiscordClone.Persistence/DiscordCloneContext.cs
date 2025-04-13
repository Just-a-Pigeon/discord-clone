using DiscordClone.Domain.Entities.Consultation;
using DiscordClone.Domain.Entities.Consultation.ServerEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiscordClone.Persistence;

public class DiscordCloneContext(DbContextOptions<DiscordCloneContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Server> Servers { get; set; }
    public DbSet<ServerNode> ServerNodes { get; set; }
    public DbSet<ServerMember> ServerMembers { get; set; }
    public DbSet<ServerRole> ServerRoles { get; set; }
    public DbSet<ServerInviteUrl> ServerInviteUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscordCloneContext).Assembly);
    }
    
    // ReSharper disable once UnusedType.Global
    internal class Factory : IDesignTimeDbContextFactory<DiscordCloneContext>
    {
        public DiscordCloneContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DiscordCloneContext>();
            optionsBuilder
                .UseNpgsql()
                .UseSnakeCaseNamingConvention();
            return new DiscordCloneContext(optionsBuilder.Options);
        }
    }
}