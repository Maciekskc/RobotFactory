using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Head
{
    public class RobotHeadConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>
    {
        public RobotHeadConstructionQueueConsumerService(ILogger<BaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>> logger, string queueName, string queueUri, string queueSasToken) : base(logger, queueName, queueUri, queueSasToken)
        {
        }
    }
}
