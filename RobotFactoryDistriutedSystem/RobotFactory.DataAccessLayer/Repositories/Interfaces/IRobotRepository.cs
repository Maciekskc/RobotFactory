using MongoDB.Driver;
using RobotFactory.DataLayer.Models;
using System.Linq.Expressions;

namespace RobotFactory.DataAccessLayer.Repositories.Interfaces
{
    public interface IRobotRepository
    {
        Task<Robot> GetRobotByIdAsync(string id);

        Task<List<Robot>> GetAllRobotsAsync();

        Task CreateRobotAsync(Robot newRobot);

        Task<UpdateResult> AddRobotComponentAsync(string robotId, Type robotComponentType,
            RobotComponent newRobotComponent);

        Task<UpdateResult> UpdateRobotProperty(string robotId, Expression<Func<Robot, object>> expresion, object value);
    }
}
