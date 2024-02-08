using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Legs
{
    public class RobotLegsConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>
    {
        public RobotLegsConstructionQueueConsumerService(ILogger<BaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>> logger, string queueName, string queueUri, string queueSasToken) : base(logger, queueName, queueUri, queueSasToken)
        {
        }
    }
}
