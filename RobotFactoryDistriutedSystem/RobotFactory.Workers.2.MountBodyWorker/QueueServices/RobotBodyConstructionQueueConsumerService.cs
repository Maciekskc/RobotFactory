using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers._2.MountBodyWorker.QueueServices
{
    public class RobotBodyConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>
    {
        public RobotBodyConstructionQueueConsumerService(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>> logger) : base(configuration, logger)
        {
        }
    }
}
