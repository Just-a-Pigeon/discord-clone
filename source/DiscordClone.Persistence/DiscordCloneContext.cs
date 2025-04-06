using DiscordClone.Domain.Entities;
using DiscordClone.Domain.Entities.Consultation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Persistence;

public class DiscordCloneContext(DbContextOptions<DiscordCloneContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscordCloneContext).Assembly);
    }
}