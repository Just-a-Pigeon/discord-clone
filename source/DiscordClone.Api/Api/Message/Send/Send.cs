﻿using FastEndpoints;

namespace DiscordClone.Api.Api.Message.Send;

public class Send : Group
{
    public Send()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Configure("send", ep =>
        {
            ep.Group<Messages>();
            ep.Description(x =>
            {
                x.RequireAuthorization(); 
            });
        });
    }
}