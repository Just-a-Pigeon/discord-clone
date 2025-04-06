using FastEndpoints;

namespace DiscordClone.Api.Api.FriendRequest;

public class FriendRequests : Group
{
    public FriendRequests()
    {
        Configure("friend-requests", ep => { });
    }
}