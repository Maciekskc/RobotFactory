using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Head
{
    public class RobotHeadConstructionQueueProducerService : BaseWorkerQueuePublisher<RobotConstructionMountHeadMessage>
    {
        public RobotHeadConstructionQueueProducerService(string queueName, string queueUri, string queueSasToken, ILogger<BaseWorkerQueuePublisher<RobotConstructionMountHeadMessage>> logger) : base(queueName, queueUri, queueSasToken, logger)
        {
        }
    }
}
