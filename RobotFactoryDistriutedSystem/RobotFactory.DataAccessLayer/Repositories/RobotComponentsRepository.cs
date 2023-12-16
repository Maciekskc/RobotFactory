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

        public Task<List<RobotComponent>> GetAllRobotComponentsByRobotIdAsync(string RobotId)
        {
            return _robotComponentsCollection.Find(rc => rc.RobotId == RobotId).ToListAsync();
        }

        public Task<RobotComponent> GetRobotComponentByIdAsync(string RobotComponentId)
        {
            return _robotComponentsCollection.Find(rc => rc.Id == RobotComponentId).FirstOrDefaultAsync();
        }
    }
}
