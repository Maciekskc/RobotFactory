using RobotFactory.DataAccessLayer.QueueServices.BaseModels;

namespace RobotFactory.Workers.ConstructionOrganizer.QueueServices
{
    public class RobotBodyConstructionQueueProducerService : BaseWorkerQueuePublisher
    {
        public RobotBodyConstructionQueueProducerService(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger) : base(configuration, logger)
        {
        }
    }
}
