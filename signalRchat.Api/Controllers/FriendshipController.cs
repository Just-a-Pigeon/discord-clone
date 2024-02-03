using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using signalRchat.Core.Services;

namespace signalRchat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly FriendshipService _friendshipService;

        public FriendshipController(FriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpPost("sendrequest")]
        [Authorize]
        public async Task<IActionResult> SendFriendRequest(Guid userId, Guid friendId)
        {
            if (userId == null || friendId == null) 
                return BadRequest("Something went wrong trying to send the friend request");

            await _friendshipService.SendFriendRequest(userId, friendId);
            return Ok("Friend request has been sent successfully");
        }

        [HttpGet("pendingrequests/{userId:guid}")]
        public async Task<IActionResult> GetPendingFriendRequests(Guid userId)
        {
            if (userId == null)
                return BadRequest("Something went wrong retrieving the friend requests");

            var pendingRequests = await _friendshipService.GetPendingFriendRequest(userId);
            return Ok(pendingRequests);

        }



    }
}
