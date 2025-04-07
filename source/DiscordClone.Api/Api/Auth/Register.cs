using System.Text.RegularExpressions;
using DiscordClone.Contract.Rest.Request.Auth;
using DiscordClone.Domain.Entities.Consultation;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace DiscordClone.Api.Api.Auth;

public class Register(UserManager<ApplicationUser> userManager) : Endpoint<RegisterRequestDto>
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

        await SendOkAsync(ct);
    }
    
    public class MyValidator : Validator<RegisterRequestDto>
    {
        public MyValidator()
        {
            RuleFor(x => x.Firstname)
                .NotEmpty()
                .WithMessage("Firstname is required")
                .MaximumLength(100)
                .WithMessage("Firstname cannot exceed 100 characters");

            RuleFor(x => x.Lastname)
                .NotEmpty()
                .WithMessage("Lastname is required")
                .MaximumLength(100)
                .WithMessage("Lastname cannot exceed 100 characters");
            
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be a valid email address")
                .MaximumLength(250)
                .WithMessage("Email cannot exceed 250 characters");
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("password is required")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters")
                .Must(p => Regex.IsMatch(p,"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]).*$"))
                .WithMessage("Password must at least contain on upper case letters, numbers and special characters");
            
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required")
                .MaximumLength(100)
                .WithMessage("Username cannot exceed 100 characters");
                
        }
    }

    public class Documentation : Summary<Register>
    {
        public Documentation()
        {

            Summary = "Creates a new account";
            Description = "Creates a new account";
            ExampleRequest = new RegisterRequestDto()
            {
               Firstname = "Pigeon",
               Lastname = "city",
               Email = "pigeon@gmail.com",
               Password = "Pigeon!2",
               Username = "ThePigeon",
            };
            Response(200, "Account was successfully created");

            Response<ErrorResponse>(401,"Account already exists");
            Response<ErrorResponse>(400,"Client side error");

        }
    }
}