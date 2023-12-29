using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.SharedComponents.QueueClientServices;

namespace RobotFactory.Workers.ConstructionOrganizer.QueueServices
{
    public class StartRobotConstructionQueueConsumerService : BaseWorkerQueueConsumer<StartRobotConstruction>
    {
        public StartRobotConstructionQueueConsumerService(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<StartRobotConstruction>> logger) : base(configuration, logger)
        {
        }
    }
}
