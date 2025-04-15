using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DiscordClone.Api.Utils;

public class AddCommonResponses : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Responses.TryAdd("400", new OpenApiResponse
        {
            Description = "Client. Something at your end went wrong."
        });
        
        operation.Responses.TryAdd("401", new OpenApiResponse
        {
            Description = "Unauthorized. Authentication required or failed."
        });

        operation.Responses.TryAdd("403", new OpenApiResponse
        {
            Description = "Forbidden. You do not have access to this resource."
        });

        operation.Responses.TryAdd("404", new OpenApiResponse
        {
            Description = "NotFound. What you try to find isn't in our database."
        });
    }
}