using System.ComponentModel.DataAnnotations;

namespace signalRChatMVC.ViewModels;

public class LoginViewModel
{
    [Display(Name = "Username")]
    public string Username { get; set; }
    [Display(Name = "Password")]
    public string Password { get; set; }
}