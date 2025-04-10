namespace DiscordClone.Domain.Entities.Consultation;

public class Group
{
    public Guid GroupId { get; set; }
    public Guid LeaderId { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public List<ApplicationUser> Members { get; set; }
}