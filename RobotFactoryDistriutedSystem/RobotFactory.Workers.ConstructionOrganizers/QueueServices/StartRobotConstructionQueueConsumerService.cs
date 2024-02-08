using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ConstructionOrganizer.QueueServices
{
    public class StartRobotConstructionQueueConsumerService : BaseWorkerQueueConsumer<StartRobotConstructionMessage>
    {
        public StartRobotConstructionQueueConsumerService(ILogger<BaseWorkerQueueConsumer<StartRobotConstructionMessage>> logger, string queueName, string queueUri, string queueSasToken) : base(logger, queueName, queueUri, queueSasToken)
        {
        }
    }
}
