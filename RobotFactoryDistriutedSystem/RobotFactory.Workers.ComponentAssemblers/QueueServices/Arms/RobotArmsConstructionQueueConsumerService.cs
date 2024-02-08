using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Arms
{
    public class RobotArmsConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>
    {
        public RobotArmsConstructionQueueConsumerService(ILogger<BaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>> logger, string queueName, string queueUri, string queueSasToken) : base(logger, queueName, queueUri, queueSasToken)
        {
        }
    }
}
