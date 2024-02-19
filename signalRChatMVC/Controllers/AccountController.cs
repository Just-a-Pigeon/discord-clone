using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;
using signalRChatMVC.Models;
using signalRChatMVC.Services.Interfaces;
using signalRChatMVC.ViewModels;

namespace signalRChatMVC.Controllers
{
 
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiService _apiService;

        public AccountController(IHttpClientFactory httpClientFactory, IApiService apiService)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
        }

        // GET: AccountController
        public async Task<ActionResult> FriendList()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            
            FriendListViewModel friendListViewModel = new FriendListViewModel();
            
            var userId =  Guid.Parse(User.Claims.Where(c=> c.Type == ClaimTypes.NameIdentifier).First().Value);
            var token = HttpContext.Session.GetString("Token");
            
            var friendlist = await _apiService.GetFriends(userId, token);

            friendListViewModel.Friends = friendlist;
            
            return View(friendListViewModel);
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            FriendListViewModel friendListViewModel = new FriendListViewModel();
            
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiEndpoint = $"http://localhost:5099/api/Account"; // Replace with your API endpoint
            
            var response = await client.GetFromJsonAsync<List<UserModel>>(apiEndpoint);
            var users = searchTerm == null ? response : response?.Where(
                u => u.Username.Contains(searchTerm) || u.Firstname.Contains(searchTerm)).ToList();

            friendListViewModel.SearchResults = users;

            return BadRequest();
        }

    }
  
}
