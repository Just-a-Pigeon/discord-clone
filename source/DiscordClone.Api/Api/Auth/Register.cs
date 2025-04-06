using DiscordClone.Contract.Rest.Request.Auth;
using DiscordClone.Domain.Entities.Consultation;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Api.Api.Auth;

public class Register(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> singInManager, IConfiguration config) : Endpoint<RegisterRequestDto>
{
    public override void Configure()
    {
        Post("register");
        Group<Auth>();
    }

    public override async Task HandleAsync(RegisterRequestDto req, CancellationToken ct)
    {
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var user = new ApplicationUser
        {
            UserName = req.Username,
            Email = req.Email,
            FirstName = req.Firstname,
            LastName = req.Lastname
        };

        user.PasswordHash = passwordHasher.HashPassword(user, req.Password);

        var result = await userManager.CreateAsync(user, req.Password);

        if (!result.Succeeded)
        {
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        await userManager.AddToRoleAsync(user, "User");

        await SendOkAsync(ct);
    }
}