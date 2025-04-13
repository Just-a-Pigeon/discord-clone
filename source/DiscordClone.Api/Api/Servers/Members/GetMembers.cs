using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Members;

public class GetMembers(DiscordCloneContext dbContext) : Endpoint<GetMembers.Request> 
{
    public override void Configure()
    {
        Get("");
        Group<Members>();
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }
        [HideFromDocs]
        public Guid UserId { get; set; }
    }
}