using DiscordClone.Business.ApiServices.Api;
using DiscordClone.Contract.Rest.Request.Auth;
using Microsoft.AspNetCore.Mvc;
using signalRChatMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace signalRChatMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IApiService _apiService;

        public AuthController(IHttpClientFactory httpClientFactory, IApiService apiService)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
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
                var response = await _apiService.Login(new LoginRequestDto
                {
                    Username = loginViewModel.Username,
                    Password = loginViewModel.Password
                });

                if (!response.IsSuccessful)
                {
                    return Unauthorized();
                }
                
                var token = response.Content.Token.ToString();
                
                if (!token.IsNullOrEmpty())
                {
                    // Store the token securely (e.g., in a cookie or session)
                    HttpContext.Session.SetString("Token", token);
                    
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

        // [AllowAnonymous]
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        // {
        //     try
        //     {
        //         bool isRegistered = await _apiService.Register(registerViewModel.Username, registerViewModel.Firstname,
        //             registerViewModel.Lastname, registerViewModel.Password, registerViewModel.Email);
        //
        //         if (isRegistered)
        //         {
        //             // Redirect to the home page or a secure area
        //             return RedirectToAction("Login", "Auth");
        //         }
        //         else
        //         {
        //             // You might want to display an error message to the user
        //             ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        //             return View();
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         // Handle exceptions (e.g., network issues, server errors)
        //         ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
        //         return View();
        //     }
        // }
    }
}