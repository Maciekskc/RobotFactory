using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
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

        public Task DeleteRobotComponentAsync(string robotComponentId)
        {
            return _robotComponentsCollection.DeleteOneAsync(rc => rc.Id == robotComponentId);
        }

        public Task<List<Body>> GetRobotBodyComponentsByRobotIdAndComponentTypeAsync(string robotId)
        {
            return _robotComponentsCollection.Find(rc => rc.RobotId == robotId && rc.ComponentType == RobotComponentType.Body)
                .Project(rc => (Body)rc)
                .ToListAsync();
        }

        public Task<List<Head>> GetRobotHeadComponentsByRobotIdAndComponentTypeAsync(string robotId)
        {
            return _robotComponentsCollection.Find(rc => rc.RobotId == robotId && rc.ComponentType == RobotComponentType.Head)
                .Project(rc => (Head)rc)
                .ToListAsync();
        }

        public Task<List<Arm>> GetRobotArmsComponentsByRobotIdAndComponentTypeAsync(string robotId)
        {
            return _robotComponentsCollection.Find(rc => rc.RobotId == robotId && rc.ComponentType == RobotComponentType.Arm)
                .Project(rc => (Arm)rc)
                .ToListAsync();
        }

        public Task<List<Leg>> GetRobotLegsComponentsByRobotIdAndComponentTypeAsync(string robotId)
        {
            return _robotComponentsCollection.Find(rc => rc.RobotId == robotId && rc.ComponentType == RobotComponentType.Leg)
                .Project(rc => (Leg)rc)
                .ToListAsync();
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
