using DiscordClone.Contract.Rest.Response.Account;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Account;

public class GetAccounts(DiscordCloneContext dbContext) : EndpointWithoutRequest<AccountResponseDto[]>
{
    public override void Configure()
    {
        Get("");
        Group<Accounts>();
    }

    public override async Task HandleAsync(CancellationToken ct)
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
}