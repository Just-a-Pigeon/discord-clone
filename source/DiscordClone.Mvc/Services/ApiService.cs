using System.Net.Http.Headers;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NuGet.Common;
using signalRChatMVC.DTOs.Auth;
using signalRChatMVC.DTOs.Messages;
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

    public async Task SendMessage(string sender, string roomName, string content, DateTime timestamp,string token)
    {
        var client = _httpClientFactory.CreateClient();
        var endpoint = baseUrl + "Message";

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var messageDto = new MessageDto
        {
            Sender = sender,
            Content = content,
            RoomName = roomName,
            Timestamp = timestamp
        };

        await client.PostAsJsonAsync(endpoint, messageDto);
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

    public async  Task<List<UserModel>> GetFriends(Guid userId,string token)
    {
        var client = _httpClientFactory.CreateClient();
        var endPoint = baseUrl + $"FriendRequest/friends/{userId}";

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetStringAsync(endPoint);
        var friends = JsonConvert.DeserializeObject<List<UserModel>>(response);

        return friends;
    }

    public async Task<List<UserModel>> GetUsers(string token)
    {
        var client = _httpClientFactory.CreateClient();
        var endPoint = baseUrl + $"Account";

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetStringAsync(endPoint);
        var users = JsonConvert.DeserializeObject<List<UserModel>>(response);

        return users;
    }
}