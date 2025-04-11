﻿using FastEndpoints;

namespace DiscordClone.Api.Api.Message;

public class Messages : FastEndpoints.Group
{
    public Messages()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("messages", ep =>
        {
            ep.Description(x =>
            {
                x.RequireAuthorization();
            });
        });
    }
}