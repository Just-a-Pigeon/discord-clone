using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using signalRchat.Core.Entities;

namespace signalRchat.Infrastructure.Data;

public class MongoMessageDbContext
{
    private readonly IMongoDatabase _database;

    public MongoMessageDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("MessageDatabase");
    }

    public IMongoCollection<Message> Messages =>
        _database.GetCollection<Message>("Messages");

}