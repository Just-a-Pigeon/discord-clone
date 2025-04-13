using Microsoft.AspNetCore.Http;

namespace DiscordClone.Contract.Rest.Request.Servers;

public class CreateServerRequestDto
{
    public string Name { get; set; } = null!;
    public IFormFile? Image { get; set; }
}