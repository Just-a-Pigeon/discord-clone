using FastEndpoints;

namespace DiscordClone.Api.Api.FriendRequests;

public class FriendRequests : Group
{
    public FriendRequests()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("friend-requests", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization();
            });
        });
    }
}