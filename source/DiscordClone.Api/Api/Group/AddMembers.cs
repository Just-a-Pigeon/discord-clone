using DiscordClone.Api.Api.Binders;
using DiscordClone.Api.Api.Message;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Group;

public class AddMembers(DiscordCloneContext dbContext) : Endpoint<AddMembers.Request>
{
    public override void Configure()
    {
        Put("{GroupId:guid}/add-members");
        Group<Groups>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var group = await dbContext.Groups.SingleOrDefaultAsync(g => g.Id == req.GroupId, ct);

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

        var members = await dbContext.Users.Where(u => req.MemberIds.Contains(u.Id)).ToListAsync(ct);

        group.Members.AddRange(members);
        await dbContext.SaveChangesAsync(ct);
        await SendOkAsync(ct);
    }

    public class Request : IHasUserId
    {
        public Guid GroupId { get; set; }
        public required List<Guid> MemberIds { get; set; } = null!;
        [HideFromDocs] public Guid UserId { get; set; }
    }
    
    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.GroupId)
                .NotEmpty()
                .WithMessage("RoomId is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("userId is required");
            
            RuleFor(x => x.MemberIds)
                .NotEmpty()
                .WithMessage("MembersIds are required");
        }
    }

    public class Documentation : Summary<AddMembers>
    {
        public Documentation()
        {
            Summary = "Add Members to Group";
            Description = "Add Members to Group";
            ExampleRequest = new Request
            {
                GroupId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                MemberIds = new List<Guid>()
            };
            Response(200, "Added members");

            Response<ErrorResponse>(401, "couldn`t add members");
            Response<ErrorResponse>(400, "Client side error");
        }
    }
}