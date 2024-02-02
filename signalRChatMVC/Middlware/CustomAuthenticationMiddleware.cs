using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class CustomAuthenticationMiddleware : IMiddleware
{
    private readonly ILogger<CustomAuthenticationMiddleware> _logger;

    public CustomAuthenticationMiddleware(ILogger<CustomAuthenticationMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Session.GetString("Token");

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var jwttoken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var userIdentity = new ClaimsIdentity(jwttoken.Claims, "jwt");
                context.User = new ClaimsPrincipal(userIdentity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing JWT token");
            }
        }

        await next.Invoke(context);
    }


  
}