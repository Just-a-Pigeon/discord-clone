using CSharpFunctionalExtensions;

namespace DiscordClone.Domain.Entities.Consultation.ServerEntities;

public class ServerNode
{
    private ServerNode()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public ServerNodeType Type { get; private set; }
    public string? ChannelTopic { get; private set; }
    public bool IsPrivate { get; private set; }
    public bool IsAgeRestricted { get; private set; }
    
    public Guid? ParentId { get; private set; }
    public ServerNode? Parent { get; private set; }
    public Guid ServerId { get; private set; }
    public Server Server { get; private set; } = null!;
    public ICollection<ServerNode> Children { get; private set; } = null!;

    public static Result<ServerNode, ValidationError> CreateCategory(CreateServerNodeCommand createCommand)
    {
        return Create(createCommand, ServerNodeType.Category);
    }

    public static Result<ServerNode, ValidationError> CreateTextChannel(CreateServerNodeCommand createCommand)
    {
        return Create(createCommand, ServerNodeType.Text);
    }

    public static Result<ServerNode, ValidationError> CreateVoiceChannel(CreateServerNodeCommand createCommand)
    {
        return Create(createCommand, ServerNodeType.Voice);
    }

    private static Result<ServerNode, ValidationError> Create(CreateServerNodeCommand createCommand,
        ServerNodeType type)
    {
        if (string.IsNullOrWhiteSpace(createCommand.Name))
            return ValidationError.InvalidInput("Name is required", "name");
        if (createCommand.Name.Length > 20)
            return ValidationError.InvalidInput("Name cannot be longer then 20 characters", "name");

        return new ServerNode
        {
            Name = createCommand.Name,
            Type = type,
            ParentId = createCommand.Parent,
            ServerId = createCommand.Server,
            IsPrivate = createCommand.IsPrivate,
            IsAgeRestricted = createCommand.IsAgeRestricted
        };
    }

    public UnitResult<ValidationError> MoveNode(ServerNode? newParent)
    {
        Parent = newParent;
        return UnitResult.Success<ValidationError>();
    }

    public record CreateServerNodeCommand
    {
        public string Name { get; set; } = null!;
        public Guid? Parent { get; set; }
        public Guid Server { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsAgeRestricted { get; set; }
    }
}