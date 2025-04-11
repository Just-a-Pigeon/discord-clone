using FastEndpoints;

namespace DiscordClone.Api.Api.FriendRequest;

public class FriendRequests : FastEndpoints.Group
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