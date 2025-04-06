using DiscordClone.Api.DTOs.Account;
using DiscordClone.Persistence;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace DiscordClone.Api.Api.Account;
public class GetAccount(DiscordCloneContext dbContext) : Endpoint<GetAccount.Request, AccountResponseDto>
{
    
    public override void Configure()
    {
        Get("{Id:Guid}");
        Group<Accounts>();
    }

    public override async Task HandleAsync(Request req, CancellationToken ct)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == req.Id, ct);
        if (user is null)
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

    public class Request
    {
        public Guid Id { get; set; }
    }
}