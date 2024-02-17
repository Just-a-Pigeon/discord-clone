using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using signalRChatMVC.Services.Interfaces;
using signalRChatMVC.ViewModels;

namespace signalRChatMVC.Controllers
{
    
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private IApiService _apiService;

        public ChatController(IHttpClientFactory httpClientFactory, IApiService apiService)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
        }
        // GET: ChatController
    
        [HttpGet]
        public async Task<ActionResult> Chat(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Auth");
            
            ViewBag.RoomName = id;
            var token = HttpContext.Session.GetString("Token");
            var messages = await _apiService.GetMessages(id, token);

            var messageVieModel = new MessageViewModel();
            messageVieModel.Messages = messages;
            
            return View(messageVieModel);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Chat(MessageViewModel messageViewModel)
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
            var token = HttpContext.Session.GetString("Token");
            await _apiService.SendMessage(messageViewModel.Sender, messageViewModel.RoomName, messageViewModel.Content,
                messageViewModel.Timestamp = DateTime.Now, token);
            return RedirectToAction("Chat");
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