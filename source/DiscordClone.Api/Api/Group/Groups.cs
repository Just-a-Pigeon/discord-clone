namespace DiscordClone.Api.Api.Group;

public class Groups : FastEndpoints.Group
{
    public Groups()
    {
        Configure("groups", ep =>
        {
            {
                ep.Description(x => { x.RequireAuthorization(); });
            }
        });
    }
}