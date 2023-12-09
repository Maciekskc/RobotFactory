using Amazon.Util.Internal.PlatformServices;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.DataAccessLayer.Repositories
{
    public class RobotRepository : IRobotRepository
    {
        private readonly IMongoCollection<Robot> _robotsCollection;

        public RobotRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["MongoDatabase:ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDatabase:DatabaseName"]);

            _robotsCollection = mongoDatabase.GetCollection<Robot>(configuration["MongoDatabase:RobotCollectionName"]);
        }

        public Task CreateRobotAsync(Robot newRobot)
        {
            return _robotsCollection.InsertOneAsync(newRobot);
        }

        public Task<List<Robot>> GetAllRobotsAsync()
        {
            return _robotsCollection.Find(_ => true).ToListAsync();
        }

        public Task<Robot> GetRobotByIdAsync(string id)
        {
            return _robotsCollection.Find(Robot => Robot.Id == id).FirstOrDefaultAsync();
        }
    }
}
