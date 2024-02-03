using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using signalRChatMVC.ViewModels;

namespace signalRChatMVC.Controllers
{
    
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        // GET: ChatController
    
        [HttpGet]
        public async Task<ActionResult> Chat(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            
            ViewBag.RoomName = id;
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiEndpoint = $"http://localhost:5099/api/Message/room/{id}"; // Replace with your API endpoint
            
            
            var response = await client.GetStringAsync(apiEndpoint);
            var messages = JsonConvert.DeserializeObject<List<MessageViewModel>>(response);
            return View(messages);
        }
        
        
        [HttpPost]
        public async Task<ActionResult> Chat()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            /*
            ViewBag.RoomName = id;
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Session.GetString("Token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var apiEndpoint = $"http://localhost:5099/api/Message/"; // Replace with your API endpoint
            
            var response = await client.PostAsJsonAsync(apiEndpoint,messageViewModel);
            */
            return View();
        }
        

        public ActionResult ChatRooms(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            
            ViewBag.RoomName = id;
            return View();
        }
    }
}