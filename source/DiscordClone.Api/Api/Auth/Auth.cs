using FastEndpoints;

namespace DiscordClone.Api.Api.Auth;

public class Auth : Group
{
    public Auth()
    {
        Configure("auth", ep =>
        {
            ep.Description(x =>
            {
                x.AllowAnonymous();
            });
        });
    }
}