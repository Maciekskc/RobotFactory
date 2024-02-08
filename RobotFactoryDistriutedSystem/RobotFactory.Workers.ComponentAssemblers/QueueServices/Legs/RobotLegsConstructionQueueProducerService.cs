using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Legs
{
    public class RobotLegsConstructionQueueProducerService : BaseWorkerQueuePublisher<RobotConstructionMountLegsMessage>
    {
        public RobotLegsConstructionQueueProducerService(string queueName, string queueUri, string queueSasToken, ILogger<BaseWorkerQueuePublisher<RobotConstructionMountLegsMessage>> logger) : base(queueName, queueUri, queueSasToken, logger)
        {
        }
    }
}
