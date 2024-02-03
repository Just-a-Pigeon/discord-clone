using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using signalRChatMVC.Models;
using signalRChatMVC.ViewModels;

namespace signalRChatMVC.Controllers
{
 
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: AccountController
        public async Task<ActionResult> FriendList()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            
            FriendListViewModel friendListViewModel = new FriendListViewModel();
            var id = User.Claims.Where(c=> c.Type == ClaimTypes.NameIdentifier).First().Value;
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiEndpoint = $"http://localhost:5099/api/FriendRequest/friends/{id}"; // Replace with your API endpoint
            
            
            var response = await client.GetStringAsync(apiEndpoint);
            friendListViewModel.Friends =JsonConvert.DeserializeObject<List<UserModel>>(response);
            return View(friendListViewModel);
        }

    }
}
