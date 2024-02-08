using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers._3.MountHead.QueueServices
{
    public class RobotHeadConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>
    {
        public RobotHeadConstructionQueueConsumerService(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>> logger) : base(configuration, logger)
        {
        }
    }
}
