using NuGet.Common;
using signalRChatMVC.DTOs.Auth;
using signalRChatMVC.Services.Interfaces;

namespace signalRChatMVC.Services;

public class ApiService: IApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    const string baseUrl = "http://localhost:5099/api/";
   
    public ApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<string> Login(string username, string password)
    {
        var client = _httpClientFactory.CreateClient();
        var endPoint = baseUrl + "/auth/Login";

        var loginDto = new LoginDto()
        {
            Username = username,
            Password = password
        };
        
        var response = await client.PostAsJsonAsync(endPoint,loginDto );
        
        var token = await response.Content.ReadFromJsonAsync<LoginResponsDto>();

        return token.Token;

    }

    public Task Register(string username, string firstname, string lastname, string password)
    {
        throw new NotImplementedException();
    }

    public Task SendMessage(string sender, string roomName, string content, DateTime timestamp)
    {
        throw new NotImplementedException();
    }

    public Task GetMessages(string roomId)
    {
        throw new NotImplementedException();
    }
}