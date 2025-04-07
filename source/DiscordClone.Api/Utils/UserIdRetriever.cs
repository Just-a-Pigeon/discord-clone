using System.Security.Claims;

namespace DiscordClone.Api.Utils;

public class UserIdRetriever(IHttpContextAccessor httpContextAccessor)
{
    public Guid GettUserIdFromClientScopes()
    {
        var http = httpContextAccessor.HttpContext;
        if (http == null)
            return Guid.Empty;
        var user = http.User;
        var claims = user.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return claims == null ? Guid.Empty : new Guid(claims.Value);
    }
}