using RobotFactory.DataAccessLayer.QueueServices.BaseModels;

namespace RobotFactory.Workers._2.MountBodyWorker.QueueServices
{
    public class RobotHeadConstructionQueueProducerService : BaseWorkerQueuePublisher
    {
        public RobotHeadConstructionQueueProducerService(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger) : base(configuration, logger)
        {
        }
    }
}
