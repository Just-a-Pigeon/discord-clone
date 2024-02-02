using System.ComponentModel.DataAnnotations;

namespace signalRChatMVC.ViewModels;

public class RegisterViewModel
{
    [Display(Name = "Firstname")]
    public string Firstname { get; set; }
    [Display(Name = "Lastname")]
    public string Lastname { get; set; }
    [Display(Name = "Email")] 
    public string Email { get; set; }
    [Display(Name = "Password")]
    public string Password { get; set; }
    [Display(Name = "Username")]
    public string Username { get; set; }

}