using RobotFactory.DataAccessLayer.QueueServices.BaseModels;

namespace RobotFactory.Workers._4.MountArms.QueueServices
{
    public class RobotLegsConstructionQueueProducerService : BaseWorkerQueuePublisher
    {
        public RobotLegsConstructionQueueProducerService(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger) : base(configuration, logger)
        {
        }
    }
}
