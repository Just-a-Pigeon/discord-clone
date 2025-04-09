using CSharpFunctionalExtensions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace DiscordClone.Domain.Entities.Consultation;

public class Message
{
    // ReSharper disable once UnusedMember.Local
    private Message()
    {
        // Required by EFCore
    }

    public Guid Id { get; private set; }
    public Guid Sender { get; private set; }
    public Guid Receiver { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public bool Edited { get; private set; }
    public MessageType Type { get; private set; }

    private static Result<Message, ValidationError> Create(Guid senderId, Guid receiverId, string content,
        DateTimeOffset createdOn, MessageType messageType)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ValidationError.InvalidInput("Contents of a message cannot be empty.", "content");
        if (content.Length > 2000)
            return ValidationError.InvalidInput("Contents of a message cannot exceed the limit of 2000 characters.",
                "content");

        return new Message
        {
            Sender = senderId,
            Receiver = receiverId,
            Content = content,
            CreatedOn = createdOn,
            Type = messageType,
            Edited = false
        };
    }

    public static Result<Message, ValidationError> CreateDm(Guid sender, Guid receiver, string content,
        DateTimeOffset createdOn)
    {
        return Create(sender, receiver, content, createdOn, MessageType.PersonalMessage);
    }

    public static Result<Message, ValidationError> CreateGroup(Guid sender, Guid receiver, string content,
        DateTimeOffset createdOn)
    {
        return Create(sender, receiver, content, createdOn, MessageType.Group);
    }

    public static Result<Message, ValidationError> CreateServer(Guid sender, Guid receiver, string content,
        DateTimeOffset createdOn)
    {
        return Create(sender, receiver, content, createdOn, MessageType.Server);
    }

    public UnitResult<ValidationError> Update(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ValidationError.InvalidInput("Contents of a message cannot be empty.", "content");
        if (content.Length > 2000)
            return ValidationError.InvalidInput("Contents of a message cannot exceed the limit of 2000 characters.",
                "content");

        Content = content;
        Edited = true;

        return UnitResult.Success<ValidationError>();
    }
}