using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Body
{
    public class RobotBodyConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>
    {
        public RobotBodyConstructionQueueConsumerService(ILogger<BaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>> logger, string queueName, string queueUri, string queueSasToken) : base(logger, queueName, queueUri, queueSasToken)
        {
        }
    }
}
