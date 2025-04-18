﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DiscordClone.Contract.Rest.Request.Auth;
using DiscordClone.Contract.Rest.Response.Auth;
using DiscordClone.Domain.Entities.Consultation;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DiscordClone.Api.Api.Auth;

public class Login(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> singInManager,
    IConfiguration config) : Endpoint<LoginRequestDto, LoginResponseDto>
{
    public override void Configure()
    {
        Post("login");
        Group<Auth>();
    }

    public override async Task HandleAsync(LoginRequestDto req, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(req.Username);
        user ??= await userManager.FindByNameAsync(req.Username);

        if (user == null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await singInManager.CheckPasswordSignInAsync(user, req.Password, false);

        if (!result.Succeeded)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var token = await GenerateTokenAsync(user);
        var serializedToken = new JwtSecurityTokenHandler().WriteToken(token);

        await SendOkAsync(new LoginResponseDto
        {
            Token = serializedToken
        }, ct);
    }


    private async Task<JwtSecurityToken> GenerateTokenAsync(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var userClaims = await userManager.GetClaimsAsync(user);
        claims.AddRange(userClaims);

        var roleClaims = await userManager.GetRolesAsync(user);
        foreach (var roleClaim in roleClaims) claims.Add(new Claim(ClaimTypes.Role, roleClaim));
        var expirationDays = config.GetValue<int>("JWTConfiguration:TokenExpirationDays");
        var signingKey = Encoding.UTF8.GetBytes(config.GetValue<string>("JWTConfiguration:SigningKey")!);
        var token = new JwtSecurityToken
        (
            config.GetValue<string>("JWTConfiguration:Issuer"),
            config.GetValue<string>("JWTConfiguration:Audience"),
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(expirationDays)),
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(signingKey),
                SecurityAlgorithms.HmacSha256)
        );
        return token;
    }
}