using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.DataAccessLayer.Repositories
{
    public class RobotComponentsRepository : IRobotComponentsRepository
    {
        private readonly IMongoCollection<RobotComponent> _robotComponentsCollection;

        public RobotComponentsRepository(IConfiguration configuration)
        {
            var mongoClient = new MongoClient(configuration["MongoDatabase:ConnectionString"]);

            var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDatabase:DatabaseName"]);

            _robotComponentsCollection = mongoDatabase.GetCollection<RobotComponent>(configuration["MongoDatabase:RobotComponentCollectionName"]);
        }

        public Task CreateRobotComponentAsync(RobotComponent newRobotComponent)
        {
            return _robotComponentsCollection.InsertOneAsync(newRobotComponent);
        }

        public Task DeleteRobotComponentAsync(string robotId)
        {
            return _robotComponentsCollection.DeleteOneAsync(rc => rc.Id == robotId);
        }

        public Task<List<RobotComponent>> GetAllRobotComponentsByRobotIdAsync(string robotId)
        {
            return _robotComponentsCollection.Find(rc => rc.RobotId == robotId).ToListAsync();
        }

        public Task<RobotComponent> GetRobotComponentByIdAsync(string robotComponentId)
        {
            return _robotComponentsCollection.Find(rc => rc.Id == robotComponentId).FirstOrDefaultAsync();
        }
    }
}
