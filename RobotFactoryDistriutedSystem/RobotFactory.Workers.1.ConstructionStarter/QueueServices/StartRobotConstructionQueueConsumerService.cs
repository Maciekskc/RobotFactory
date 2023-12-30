using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ConstructionOrganizer.QueueServices
{
    public class StartRobotConstructionQueueConsumerService : BaseWorkerQueueConsumer<StartRobotConstruction>
    {
        public StartRobotConstructionQueueConsumerService(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<StartRobotConstruction>> logger) : base(configuration, logger)
        {
        }
    }
}
