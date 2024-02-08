using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices.Interfaces
{
    public interface IStartRobotConstructionService
    {
        Task AddMessageToQueue(StartRobotConstructionMessage robotConstructionRequest);
    }
}
