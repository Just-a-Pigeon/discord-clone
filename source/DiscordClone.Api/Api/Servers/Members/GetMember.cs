using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;

namespace DiscordClone.Api.Api.Servers.Members;

public class GetMember(DiscordCloneContext dbContext) : Endpoint<GetMember.Request> 
{
    public override void Configure()
    {
        Get("{MemberId:guid}");
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