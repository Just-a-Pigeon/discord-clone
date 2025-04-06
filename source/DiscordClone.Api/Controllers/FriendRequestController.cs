// using DiscordClone.Core.Services.Interfaces;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace DiscordClone.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class FriendRequestController : ControllerBase
//     {
//         private readonly IFriendshipService _friendshipService;
//
//         public FriendRequestController(IFriendshipService friendshipService)
//         {
//             _friendshipService = friendshipService;
//         }
//
//         [HttpPost("sendrequest")]
//         [Authorize]
//         public async Task<IActionResult> SendFriendRequest(Guid userId, Guid friendId)
//         {
//             await _friendshipService.SendFriendRequest(userId, friendId);
//             return Ok("Friend request has been sent successfully");
//         }
//
//         [HttpGet("pendingrequests/{userId:guid}")]
//         [Authorize]
//         public async Task<IActionResult> GetPendingFriendRequests(Guid userId)
//         {
//             if (userId == null)
//                 return BadRequest("Something went wrong retrieving the friend requests");
//
//             var pendingRequests = await _friendshipService.GetPendingFriendRequest(userId);
//             return Ok(pendingRequests);
//
//         }
//
//         [HttpPost("acceptrequest")]
//         public async Task<IActionResult> AcceptFriendRequest(Guid userId, Guid friendId)
//         {
//             if (userId == null || friendId == null) 
//                 return BadRequest("Something went wrong trying to accept the friend request");
//             
//             await _friendshipService.AcceptFriendRequest(userId, friendId);
//             return Ok("Friend request accepted successfully");
//
//         }
//         
//         [HttpPost("rejectrequest")]
//         [Authorize]
//         public async Task<IActionResult> RejectFriendRequest(Guid userId, Guid friendId)
//         {
//             if (userId == null || friendId == null) 
//                 return BadRequest("Something went wrong trying to reject the friend request");
//             
//             await _friendshipService.RejectFriendRequest(userId, friendId);
//             return Ok("Friend request rejected successfully");
//
//         }
//
//         [HttpGet("friends/{userId:guid}")]
//         public async Task<IActionResult> GetFriends(Guid userId)
//         {
//             var friends = await _friendshipService.GetFriends(userId);
//             return Ok(friends);
//         }
//         
//     }
// }
