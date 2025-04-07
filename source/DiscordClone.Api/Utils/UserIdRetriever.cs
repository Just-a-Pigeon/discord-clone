namespace DiscordClone.Api.Utils;

public class UserIdRetriever(IHttpContextAccessor httpContextAccessor)
{
    public Guid? GettUserIdFromClientScopes()
    {
        //TODO: Get user id from the incoming scopes (JWT)
        return Guid.NewGuid();
    }
}