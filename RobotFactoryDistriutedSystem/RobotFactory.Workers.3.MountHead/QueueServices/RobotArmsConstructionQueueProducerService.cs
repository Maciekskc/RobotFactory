using RobotFactory.DataAccessLayer.QueueServices.BaseModels;

namespace RobotFactory.Workers._3.MountHead.QueueServices
{
    public class RobotArmsConstructionQueueProducerService : BaseWorkerQueuePublisher
    {
        public RobotArmsConstructionQueueProducerService(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger) : base(configuration, logger)
        {
        }
    }
}
