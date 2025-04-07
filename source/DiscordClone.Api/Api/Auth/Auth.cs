using FastEndpoints;

namespace DiscordClone.Api.Api.Auth;

public class Auth : Group
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