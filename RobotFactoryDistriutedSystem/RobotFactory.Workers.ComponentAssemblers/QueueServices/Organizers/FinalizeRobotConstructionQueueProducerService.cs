using RobotFactory.DataAccessLayer.QueueServices.BaseModels;

namespace RobotFactory.Workers.ComponentAssemblers.QueueServices.Organizers
{
    public class FinalizeRobotConstructionQueueProducerService<FinalizeRobotConstructionMessage> : BaseWorkerQueuePublisher<FinalizeRobotConstructionMessage>
    {
        public FinalizeRobotConstructionQueueProducerService(string queueName, string queueUri, string queueSasToken, ILogger<BaseWorkerQueuePublisher<FinalizeRobotConstructionMessage>> logger) : base(queueName, queueUri, queueSasToken, logger)
        {
        }
    }
}
