using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers._2.MountLegsWorker.QueueServices
{
    public class RobotLegsConstructionQueueConsumerService : BaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>
    {
        public RobotLegsConstructionQueueConsumerService(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>> logger) : base(configuration, logger)
        {
        }
    }
}
