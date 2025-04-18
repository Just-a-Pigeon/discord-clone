﻿namespace DiscordClone.Contract.Rest.Response.Account;

public class AccountResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string Lastname { get; set; } = null!;
    public int AmountOfPages { get; set; }
    public int Page { get; set; }
}