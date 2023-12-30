using System.Text.Json;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.SharedComponents;

namespace RobotFactory.Workers.ConstructionOrganizer
{
    public class StartRobotConstructionWorker : BaseRobotConstructionWorker<StartRobotConstruction>
    {
        private readonly ILogger<StartRobotConstructionWorker> _logger;

        public StartRobotConstructionWorker(ILogger<BaseRobotConstructionWorker<StartRobotConstruction>> logger, IBaseWorkerQueueConsumer<StartRobotConstruction> queueConsumer, IBaseWorkerQueuePublisher queuePublisher, IConfiguration configuration, ILogger<StartRobotConstructionWorker> logger1)
            : base(logger, queueConsumer, queuePublisher, configuration)
        {
            _logger = logger1;
        }

        protected override async Task<string> ExecuteQueueActionAsync(StartRobotConstruction message)
        {
            _logger.LogInformation("Worker is processing the request");

            //Here worker should do some actions instead of dumb delay
            var newMessageModel = new RobotConstructionMountBodyMessage()
            {
                RobotId = message.RobotId,
            };

            return JsonSerializer.Serialize(newMessageModel);
        }

    }
}
