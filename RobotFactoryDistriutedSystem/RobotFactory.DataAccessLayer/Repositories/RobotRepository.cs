using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using static MongoDB.Driver.WriteConcern;

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

        public async Task<UpdateResult> AddRobotComponentAsync(string robotId, Type robotComponentType, RobotComponent newRobotComponent)
        {
            var filter = Builders<Robot>.Filter.Eq(robot => robot.Id, robotId);

            var update = robotComponentType.Name switch
            {
                nameof(Head) => Builders<Robot>.Update.Set(robot => robot.Head, newRobotComponent),
                nameof(Body) => Builders<Robot>.Update.Set(robot => robot.Body, newRobotComponent),
                nameof(Arm) => Builders<Robot>.Update.AddToSet(robot => robot.Arms, newRobotComponent),
                nameof(Leg) => Builders<Robot>.Update.AddToSet(robot => robot.Legs, newRobotComponent),
                _ => throw new ArgumentOutOfRangeException("Not known type of robot component")
            };
            return await _robotsCollection.UpdateOneAsync(filter,update);
        }

        public async Task<UpdateResult> UpdateRobotProperty(string robotId, UpdateDefinition<Robot> updateDefinition)
        {
            var filter = Builders<Robot>.Filter.Eq(robot => robot.Id, robotId);
            return await _robotsCollection.UpdateOneAsync(filter, updateDefinition);
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
