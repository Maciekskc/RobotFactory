using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Arms
{
    public class RobotArmsConstructionQueueProducerService : BaseWorkerQueuePublisher<RobotConstructionMountArmsMessage>
    {
        public RobotArmsConstructionQueueProducerService(string queueName, string queueUri, string queueSasToken, ILogger<BaseWorkerQueuePublisher<RobotConstructionMountArmsMessage>> logger) : base(queueName, queueUri, queueSasToken, logger)
        {
        }
    }
}
