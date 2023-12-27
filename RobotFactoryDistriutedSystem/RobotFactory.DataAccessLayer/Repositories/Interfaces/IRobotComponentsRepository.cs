using RobotFactory.DataLayer.Models;

namespace RobotFactory.DataAccessLayer.Repositories.Interfaces
{
    public interface IRobotComponentsRepository
    {
        Task<RobotComponent> GetRobotComponentByIdAsync(string RobotComponentId);

        Task<List<RobotComponent>> GetAllRobotComponentsByRobotIdAsync(string RobotId);

        Task CreateRobotComponentAsync(RobotComponent newRobotComponent);
    }
}
