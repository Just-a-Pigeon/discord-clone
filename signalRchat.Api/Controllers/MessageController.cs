using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using signalRchat.Core.Entities;
using signalRchat.Infrastructure.Data;

namespace signalRchat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly MongoMessageDbContext _mongoDbContext;

        public MessageController(MongoMessageDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }
        
        [HttpGet("room/{roomName}")]
        [Authorize(Policy = "CanRead")]
        public IActionResult GetMessages(string roomName)
        {
            // Retrieve messages from MongoDB
            var messages = _mongoDbContext.Messages
                .Find(m => m.RoomName == roomName)
                .ToList();
            return Ok(messages);
        }
        
        [HttpGet("user/{username}")]
        [Authorize(Policy = "CanRead")]
        public IActionResult GetMessagesByUser(string username)
        {
            // Retrieve messages from MongoDB
            var messages = _mongoDbContext.Messages
                .Find(m => m.Sender == username)
                .ToList();
            return Ok(messages);
        }
        
        [HttpPost]
        public IActionResult AddMessage([FromBody] Message message)
        {
            // Store the message in MongoDB
            _mongoDbContext.Messages.InsertOne(message);
            return Ok();
        }
    }
}
