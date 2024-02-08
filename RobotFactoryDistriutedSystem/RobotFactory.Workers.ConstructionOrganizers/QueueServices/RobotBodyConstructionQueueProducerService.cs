using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.ConstructionOrganizers.QueueServices
{
    public class RobotBodyConstructionQueueProducerService : BaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>
    {
        public RobotBodyConstructionQueueProducerService(string queueName, string queueUri, string queueSasToken, ILogger<BaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>> logger) : base(queueName, queueUri, queueSasToken, logger)
        {
        }
    }
}
