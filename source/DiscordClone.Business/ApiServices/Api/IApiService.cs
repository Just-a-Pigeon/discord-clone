using DiscordClone.Contract.Rest.Request.Auth;
using DiscordClone.Contract.Rest.Response.Auth;
using Refit;

namespace DiscordClone.Business.ApiServices.Api;

public interface IApiService
{
    [Post("/api/v1/auth/login")]
    Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);
}