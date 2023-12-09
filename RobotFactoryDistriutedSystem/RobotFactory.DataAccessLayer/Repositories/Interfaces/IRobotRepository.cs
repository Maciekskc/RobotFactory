using RobotFactory.DataLayer.Models;

namespace RobotFactory.DataAccessLayer.Repositories.Interfaces
{
    public interface IRobotRepository
    {
        Task<Robot> GetRobotByIdAsync(string id);

        Task<List<Robot>> GetAllRobotsAsync();

        Task CreateRobotAsync(Robot newRobot);
    }
}
