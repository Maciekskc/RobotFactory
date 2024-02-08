using System.Text.Json;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.SharedComponents;

namespace RobotFactory.Workers.ConstructionOrganizers
{
    public class StartRobotConstructionWorker : BaseRobotConstructionWorker<StartRobotConstructionMessage,RobotConstructionMountBodyMessage>
    {
        private readonly ILogger<StartRobotConstructionWorker> _logger;

        public StartRobotConstructionWorker(ILogger<BaseRobotConstructionWorker<StartRobotConstructionMessage, RobotConstructionMountBodyMessage>> logger, IBaseWorkerQueueConsumer<StartRobotConstructionMessage> queueConsumer, IBaseWorkerQueuePublisher<RobotConstructionMountBodyMessage> queuePublisher, IConfiguration configuration, IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository, ILogger<StartRobotConstructionWorker> logger1) : base(logger, queueConsumer, queuePublisher, configuration, robotComponentsRepository, robotRepository)
        {
            _logger = logger1;
        }
        protected override async Task<string> ExecuteQueueActionAsync(StartRobotConstructionMessage message)
        {
            _logger.LogInformation("Worker is processing the request");

            var robotObject = await RobotRepository.GetRobotByIdAsync(message.RobotId);
            if (robotObject == null)
                throw new ArgumentException("Robot with given Id does not exist");

            await RobotRepository.UpdateRobotProperty(robotObject.Id, robot => robot.ConstructionStatus,
                RobotConstrucionStatus.ConstructionStarted);
            var newMessageModel = new RobotConstructionMountBodyMessage()
            {
                RobotId = robotObject.Id
            };

            return JsonSerializer.Serialize(newMessageModel);
        }
    }
}
