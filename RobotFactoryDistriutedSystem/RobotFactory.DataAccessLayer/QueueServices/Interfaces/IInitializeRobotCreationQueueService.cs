using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices.Interfaces
{
    public interface IInitializeRobotCreationQueueService
    {
        Task AddMessageToQueue(InitializeRobotCreation robotId);
    }
}
