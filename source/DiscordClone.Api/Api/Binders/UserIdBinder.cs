using DiscordClone.Api.Utils;
using FastEndpoints;

namespace DiscordClone.Api.Api.Binders;

public class UserIdBinder<TRequest>(UserIdRetriever userRetriever) : RequestBinder<TRequest> where TRequest : notnull
{
    public override async ValueTask<TRequest> BindAsync(BinderContext ctx, CancellationToken ct)
    {
        var req = await base.BindAsync(ctx, ct);

        if (req is IHasUserId userIdRequest)
            userIdRequest.UserId = userRetriever.GettUserIdFromClientScopes();
        
        return req;
    }
}