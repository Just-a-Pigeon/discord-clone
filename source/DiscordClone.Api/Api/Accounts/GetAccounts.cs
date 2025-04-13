using DiscordClone.Api.Api.Binders;
using DiscordClone.Contract.Rest.Response.Accounts;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Accounts;

public class GetAccounts(DiscordCloneContext dbContext) : Endpoint<GetAccounts.Request, AccountResponseDto[]>
{
    public override void Configure()
    {
        Get("");
        Group<Accounts>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var users = await dbContext.Users.ToListAsync(ct);
        var result = users.Select(u => new AccountResponseDto
        {
            Id = u.Id,
            Username = u.UserName ?? string.Empty,
            Firstname = u.FirstName,
            Lastname = u.LastName
        }).ToArray();
        
        await SendOkAsync(result, ct);
    }

    public class Request : IHasUserId
    {
        [HideFromDocs]
        public Guid UserId { get; set; }
    }
}