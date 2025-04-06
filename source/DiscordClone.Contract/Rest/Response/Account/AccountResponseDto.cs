namespace DiscordClone.Api.DTOs.Account;

public class AccountResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
}