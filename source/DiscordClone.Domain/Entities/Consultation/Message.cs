using CSharpFunctionalExtensions;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace DiscordClone.Domain.Entities.Consultation;

public class Message
{
    public Guid Id { get; private set; }
    public Guid Sender { get; private set; }
    public Guid Receiver { get; private set; }
    public string Content { get; private set; }
    public DateTimeOffset CreatedOn { get; private set; }
    public bool Edited { get; private set; }
    public MessageType Type { get; private set; }
    
    // ReSharper disable once UnusedMember.Local
    private Message()
    {
        // Required by EFCore
    }
    
    public static Result<Message, ValidationError> CreateDm(Guid sender, Guid receiver, string content, DateTimeOffset createdOn)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ValidationError.InvalidInput("Contents of a message cannot be empty.", "content");
        if (content.Length > 2000)
            return ValidationError.InvalidInput("Contents of a message cannot exceed the limit of 2000 characters.", "content");
        
        return new Message
        {
            Sender = sender,
            Receiver = receiver,
            Content = content,
            CreatedOn = createdOn,
            Type = MessageType.PersonalMessage,
            Edited = false
        };
    }
    
    public static Result<Message, ValidationError> CreateGroup(Guid sender, Guid receiver, string content, DateTimeOffset createdOn)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ValidationError.InvalidInput("Contents of a message cannot be empty.", "content");
        if (content.Length > 2000)
            return ValidationError.InvalidInput("Contents of a message cannot exceed the limit of 2000 characters.", "content");
        
        return new Message
        {
            Sender = sender,
            Receiver = receiver,
            Content = content,
            CreatedOn = createdOn,
            Type = MessageType.Group,
            Edited = false
        };
    }
    
    public static Result<Message, ValidationError> CreateServer(Guid sender, Guid receiver, string content, DateTimeOffset createdOn)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ValidationError.InvalidInput("Contents of a message cannot be empty.", "content");
        if (content.Length > 2000)
            return ValidationError.InvalidInput("Contents of a message cannot exceed the limit of 2000 characters.", "content");
        
        return new Message
        {
            Sender = sender,
            Receiver = receiver,
            Content = content,
            CreatedOn = createdOn,
            Type = MessageType.Server,
            Edited = false
        };
    }

    public UnitResult<ValidationError> Update(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return ValidationError.InvalidInput("Contents of a message cannot be empty.", "content");
        if (content.Length > 2000)
            return ValidationError.InvalidInput("Contents of a message cannot exceed the limit of 2000 characters.", "content");
    
        Content = content;
        Edited = true;
        
        return UnitResult.Success<ValidationError>();
    }
}