using Microsoft.AspNetCore.Http;

namespace DiscordClone.Contract.Rest.Request.Servers;

public class UpdateServerRequestDto
{
    public string? Name { get; set; } = null!;
    public IFormFile? Image { get; set; }
    public string? Description { get; set; }
    public IFormFile? BannerImagePath { get; set; }
}