using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using signalRChatMVC.Models;
using signalRChatMVC.ViewModels;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace signalRChatMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: AuthController
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            LoginViewModel loginViewModel = new();
            return View(loginViewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiEndpoint = "http://localhost:5099/api/auth/Login"; // Replace with your API endpoint

                var response = await client.PostAsJsonAsync(apiEndpoint, loginViewModel);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadFromJsonAsync<AuthLoginModel>();
                    // Store the token securely (e.g., in a cookie or session)
                    HttpContext.Session.SetString("Token", token.Token);
                    
                    // Redirect to the home page or a secure area
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Handle unsuccessful login
                    // You might want to display an error message to the user
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network issues, server errors)
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login","Auth");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            RegisterViewModel registerViewModel = new();
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var apiEndpoint = "http://localhost:5099/api/auth/Register"; // Replace with your API endpoint

                var response = await client.PostAsJsonAsync(apiEndpoint, registerViewModel);

                if (response.IsSuccessStatusCode)
                {
                    // Redirect to the home page or a secure area
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    // Handle unsuccessful login
                    // You might want to display an error message to the user
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., network issues, server errors)
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View();
            }
        }
    }
}