using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation;

public class Group
{
    private Group()
    {
    }

    public Guid GroupId { get; private set; }
    public Guid LeaderId { get; private set; }
    public string Name { get; private set; }
    public string? ImagePath { get; private set; }
    public List<ApplicationUser> Members { get; private set; }

    private static string DefaultName(List<ApplicationUser> members)
    {
        var membersNames = members.Select(m => m.UserName);
        return string.Join(", ", membersNames);
    }

    public static Result<Group, ValidationError> CreateGroup(Guid leaderId, string imagePath,
        List<ApplicationUser> members)
    {
        return new Group
        {
            LeaderId = leaderId,
            Name = DefaultName(members),
            ImagePath = null,
            Members = members
        };
    }

    public UnitResult<ValidationError> Update(Guid leaderId, string name, string imagePath,
        List<ApplicationUser> members)
    {
        if (name.Length > 100)
            return ValidationError.InvalidInput("name of the group cannot exceed the limit of 100 characters.",
                "name");
        if (string.IsNullOrWhiteSpace(name))
            Name = DefaultName(members);

        LeaderId = leaderId;
        Name = name;
        ImagePath = imagePath;
        Members = members;

        return UnitResult.Success<ValidationError>();
    }
}