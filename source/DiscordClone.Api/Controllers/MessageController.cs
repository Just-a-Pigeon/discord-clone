// using DiscordClone.Domain.Entities.Consultation;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace DiscordClone.Api.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class MessageController(MongoMessageDbContext mongoDbContext) : ControllerBase
//     {
//         [HttpGet("room/{roomName}")]
//         [Authorize(Policy = "CanRead")]
//         public IActionResult GetMessages(string roomName) /getmessages
//         {
//             // Retrieve messages from MongoDB
//             var messages = mongoDbContext.Messages
//                 .Find(m => m.RoomName == roomName)
//                 .ToList();
//             return Ok(messages);
//         }
//         
//         [HttpGet("user/{username}")]
//         [Authorize(Policy = "CanRead")]
//         public IActionResult GetMessagesByUser(string username) /getmessagesbyuser
//         {
//             // Retrieve messages from MongoDB
//             var messages = mongoDbContext.Messages
//                 .Find(m => m.Sender == username)
//                 .ToList();
//             return Ok(messages);
//         }
//         
//         [HttpPost]
//         public IActionResult AddMessage([FromBody] Message message) /SendMessage
//         {
//             // Store the message in MongoDB
//             mongoDbContext.Messages.InsertOne(message);
//             return Ok();
//         }
//     }
// }
