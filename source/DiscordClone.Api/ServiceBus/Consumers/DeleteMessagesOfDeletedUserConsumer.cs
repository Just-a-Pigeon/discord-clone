using DiscordClone.Api.ServiceBus.Commands;
using DiscordClone.Persistence;
using MassTransit;

namespace DiscordClone.Api.ServiceBus.Consumers;

public class DeleteMessagesOfDeletedUserConsumer(DiscordCloneContext dbContext): IConsumer<DeleteMessagesOfDeletedUser>
{
    /// <summary>
    /// code that wil be executed 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task Consume(ConsumeContext<DeleteMessagesOfDeletedUser> context) 
    {
        
        //TODO
        // Implement logic to delete user 
        throw new NotImplementedException();
    }
}