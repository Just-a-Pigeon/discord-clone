using DiscordClone.Api.Api.Binders;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Group;

public class RemoveMembers(DiscordCloneContext dbContext) : Endpoint<RemoveMembers.Request>
{
    public override void Configure()
    {
        Put("{GroupId:guid}/remove/{MemberId:guid}");
        Group<Groups>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var group = await dbContext.Groups.Include(group => group.Members)
            .SingleOrDefaultAsync(g => g.Id == req.GroupId, ct);

        if (group == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (group.LeaderId != req.UserId)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var member = group.Members.SingleOrDefault(m => m.Id == req.MemberId);

        if (member is not null)
            group.Members.Remove(member);

        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs] public Guid UserId { get; set; }
        [HideFromDocs] public Guid MemberId { get; set; }
        public Guid GroupId { get; set; }
    }

    public class MyValidator : Validator<RemoveMembers.Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithMessage("RoomId is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("userId is required");

            RuleFor(x => x.MemberId)
                .NotEmpty()
                .WithMessage("MemberId is required");
        }
    }

    public class Documentation : Summary<RemoveMembers>
    {
        public Documentation()
        {
            Summary = "Remove a member of the group";
            Description = "Remove a member of the group";
            ExampleRequest = new RemoveMembers.Request
            {
                GroupId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                MemberId = Guid.NewGuid()
            };
            Response(200, "Removed member of the group");

            Response<ErrorResponse>(401, "couldn`t remove the member of the group");
            Response<ErrorResponse>(400, "Client side error");
        }
    }
}