using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.DataAccessLayer.Repositories.Interfaces
{
    public interface IRobotComponentsRepository
    {
        Task<RobotComponent> GetRobotComponentByIdAsync(string robotComponentId);
        Task<List<Body>> GetRobotBodyComponentsByRobotIdAndComponentTypeAsync(string robotId);
        Task<List<Head>> GetRobotHeadComponentsByRobotIdAndComponentTypeAsync(string robotId);
        Task<List<Arm>> GetRobotArmsComponentsByRobotIdAndComponentTypeAsync(string robotId);
        Task<List<Leg>> GetRobotLegsComponentsByRobotIdAndComponentTypeAsync(string robotId);
        Task<List<RobotComponent>> GetAllRobotComponentsByRobotIdAsync(string robotId);
        Task CreateRobotComponentAsync(RobotComponent newRobotComponent);
        Task DeleteRobotComponentAsync(string robotComponentId);
    }
}
