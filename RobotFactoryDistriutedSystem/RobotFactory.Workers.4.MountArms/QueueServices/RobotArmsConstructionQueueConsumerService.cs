using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers._4.MountArms.QueueServices
{
    public class RobotArmsConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>
    {
        public RobotArmsConstructionQueueConsumerService(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>> logger) : base(configuration, logger)
        {
        }
    }
}
