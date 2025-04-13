using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Members;

public class KickMember(DiscordCloneContext dbContext) : Endpoint<KickMember.Request> 
{
    public override void Configure()
    {
        Put("{MemberId:guid}/kick");
        Group<Members>();
    }

    public class Request : IHasUserId
    {
        public Guid ServerId { get; set; }
        public Guid MemberId { get; set; }
        [HideFromDocs]
        public Guid UserId { get; set; }
    }
}