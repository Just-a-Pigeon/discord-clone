using FastEndpoints;

namespace DiscordClone.Api.Api.Auth;

public class Auth: FastEndpoints.Group
{
    public Auth()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("auth", ep =>
        {
            ep.Description(x =>
            {
                x.AllowAnonymous();
            });
        });
    }
}