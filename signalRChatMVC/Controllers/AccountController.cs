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

        public async Task<IActionResult> Index(string searchTerm)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            
            FriendListViewModel friendListViewModel = new FriendListViewModel();

            friendListViewModel.Friends = await GetFriendList();
            friendListViewModel.SearchResults = await Search(searchTerm);
            
            return View(friendListViewModel);
        }
        
        public async Task<List<UserModel>> GetFriendList()
        {
            
            var userId =  Guid.Parse(User.Claims.Where(c=> c.Type == ClaimTypes.NameIdentifier).First().Value);
            var token = HttpContext.Session.GetString("Token");
            
            var friendlist = await _apiService.GetFriends(userId, token);

            return friendlist;
        }
        
        public async Task<List<UserModel>> Search(string searchTerm)
        {
            
            
            var token = HttpContext.Session.GetString("Token");
            var users= await _apiService.GetUsers(token);
          
            var usersList = searchTerm == null ? users : users?.Where(
                u => u.Username.Contains(searchTerm) || u.Firstname.Contains(searchTerm)).ToList();
            
            return usersList;
        }

    }
  
}
