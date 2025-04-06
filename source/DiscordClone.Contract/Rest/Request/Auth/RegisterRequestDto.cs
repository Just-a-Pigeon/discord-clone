using System.ComponentModel.DataAnnotations;

namespace DiscordClone.Api.DTOs.Auth;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "Please enter your first name")]
    public string Firstname { get; set; }

    [Required(ErrorMessage = "Please enter your last name")]
    public string Lastname { get; set; }

    [Required(ErrorMessage = "Please enter your email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please enter a username")]
    public string Username { get; set; }
}