using System.Net.Http.Headers;
using Newtonsoft.Json;
using NuGet.Common;
using signalRChatMVC.DTOs.Auth;
using signalRChatMVC.Models;
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
        var endPoint = baseUrl + "auth/Login";

        var loginDto = new LoginDto
        {
            Username = username,
            Password = password
        };
        
        var response = await client.PostAsJsonAsync(endPoint,loginDto );
        
        var token = await response.Content.ReadFromJsonAsync<LoginResponsDto>();

        return token.Token;

    }

    public async Task<bool> Register(string username, string firstname, string lastname, string password,string email)
    {
        var client = _httpClientFactory.CreateClient();
        var endPoint = baseUrl + "auth/Register";

        var registerDto = new RegisterDto
        {
            Firstname = firstname,
            Lastname = lastname,
            Email = email,
            Password = password,
            Username = username
        };

       var response = await client.PostAsJsonAsync(endPoint, registerDto);

       return response.IsSuccessStatusCode;
    }

    public Task SendMessage(string sender, string roomName, string content, DateTime timestamp)
    {
        throw new NotImplementedException();
    }

    public async Task<List<MessageModel>> GetMessages(string roomId,string token)
    {
        var client = _httpClientFactory.CreateClient();
        var endPoint = baseUrl + $"Message/room/{roomId}";

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetStringAsync(endPoint);
        var messages = JsonConvert.DeserializeObject<List<MessageModel>>(response);

        return messages;

    }
}