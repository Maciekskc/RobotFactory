using RobotFactory.Workers.SharedComponents.QueueClientServices;

namespace RobotFactory.Workers.ConstructionOrganizer.QueueServices
{
    public class RobotBodyConstructionQueueProducerService : BaseWorkerQueuePublisher
    {
        public RobotBodyConstructionQueueProducerService(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger) : base(configuration, logger)
        {
        }
    }
}
