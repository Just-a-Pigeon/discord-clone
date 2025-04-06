using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Domain.Entities.Consultation;

public class ApplicationUser: IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public virtual ICollection<Friendship> Friendships { get; set; }
}