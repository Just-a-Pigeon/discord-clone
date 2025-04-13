namespace DiscordClone.Contract.Rest.Response.Servers;

public class GetServersResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? ImagePath { get; set; }
}