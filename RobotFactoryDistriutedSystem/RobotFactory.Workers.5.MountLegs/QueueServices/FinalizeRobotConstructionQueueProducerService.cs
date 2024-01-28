using RobotFactory.DataAccessLayer.QueueServices.BaseModels;

namespace RobotFactory.Workers._5.MountLegs.QueueServices
{
    public class FinalizeRobotConstructionQueueProducerService : BaseWorkerQueuePublisher
    {
        public FinalizeRobotConstructionQueueProducerService(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger) : base(configuration, logger)
        {
        }
    }
}
