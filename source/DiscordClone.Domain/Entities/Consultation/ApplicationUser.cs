using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Domain.Entities.Consultation;

public class ApplicationUser: IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public virtual ICollection<Friendship> Friends { get; set; } = null!;
    public virtual ICollection<Friendship> FriendOf { get; set; } = null!;
    
    public virtual ICollection<Group> Groups { get; set; } = null!;
}