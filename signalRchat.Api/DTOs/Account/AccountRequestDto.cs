namespace signalRchat.Api.DTOs.Account;

public class AccountRequestDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}