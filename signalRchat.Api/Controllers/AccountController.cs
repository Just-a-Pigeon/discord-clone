using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using signalRchat.Api.DTOs.Account;
using signalRchat.Core.Services.Interfaces;

namespace signalRchat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        protected readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Authorize(Policy = "CanRead")]
        public async Task<IActionResult> Get()
        {
            var users = await _accountService.GetAllUsers();
            var userDto = users.Select(u => new AccountResponseDto
            {
                Id = u.Id,
                Firstname = u.FirstName,
                Lastname = u.LastName,
                Username = u.UserName
            });
            return Ok(userDto);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = "CanRead")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _accountService.GetUserById(id);
            if (user == null) return NotFound($"User with id: {id} not found ");

            var userDto = new AccountResponseDto
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Username = user.UserName
            };

            return Ok(userDto);
        }


    }
}
