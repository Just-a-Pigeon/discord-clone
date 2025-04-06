using FastEndpoints;

namespace DiscordClone.Api.Api.Account;

public class Accounts : Group
{
    public Accounts()
    {
        Configure("accounts", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization();
            });
        });
    }
}