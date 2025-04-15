using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Accounts;
using DiscordClone.Persistence;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Accounts;
public class GetAccount(DiscordCloneContext dbContext) : Endpoint<GetAccount.Request, AccountResponseDto>
{
    public override void Configure()
    {
        Get("{Id:Guid}");
        Group<Accounts>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await dbContext.Users
            .Include(u => u.BlockedUsers)
            .FirstOrDefaultAsync(u => u.Id == req.Id, ct);
        
        if (user is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (user.BlockedUsers.Any(b => b.Id == req.UserId))
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var result = new AccountResponseDto
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Firstname = user.FirstName,
            Lastname = user.LastName
        };
        
        await SendOkAsync(result, ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs]
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }

    public class MyValidator : Validator<Request>
    {
        public MyValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }

    public class Documentation : Summary<GetAccount>
    {
        public Documentation()
        {
            var id = Guid.NewGuid();
            
            Summary = "Get account by specified Id";
            Description = "Get account by specified Id";
            ExampleRequest = new Request()
            {
                Id = id,
            };
            Response(200, "Get account by specified Id", example: new AccountResponseDto
            {
                Id = id,
                Username = "Pigeon",
                Firstname = "I stuff my",
                Lastname = "self with carrots"
            });
        }
    }
}