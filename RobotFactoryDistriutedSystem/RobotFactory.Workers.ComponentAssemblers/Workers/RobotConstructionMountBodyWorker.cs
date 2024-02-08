using System.Data;
using System.Text.Json;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.SharedComponents;

namespace RobotFactory.Workers.ComponentAssemblers.Workers
{
    public class RobotConstructionMountBodyWorker : BaseRobotConstructionWorker<RobotConstructionMountBodyMessage, RobotConstructionMountHeadMessage>
    {
        private readonly ILogger<RobotConstructionMountBodyWorker> _logger;

        public RobotConstructionMountBodyWorker(
            ILogger<BaseRobotConstructionWorker<RobotConstructionMountBodyMessage, RobotConstructionMountHeadMessage>> logger,
            IBaseWorkerQueueConsumer<RobotConstructionMountBodyMessage> queueConsumer,
            IBaseWorkerQueuePublisher<RobotConstructionMountHeadMessage> queuePublisher, IConfiguration configuration,
            IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository, ILogger<RobotConstructionMountBodyWorker> logger1) : base(logger,
            queueConsumer, queuePublisher, configuration, robotComponentsRepository, robotRepository)
        {
            _logger = logger1;
        }

        protected override async Task<string> ExecuteQueueActionAsync(RobotConstructionMountBodyMessage message)
        {
            _logger.LogInformation("{0} is processing the request", nameof(RobotConstructionMountBodyWorker));

            var robotObject = await RobotRepository.GetRobotByIdAsync(message.RobotId);
            if (robotObject == null)
                throw new ArgumentException("Robot with given Id does not exist");

            if (robotObject.ConstructionStatus != RobotConstrucionStatus.ConstructionStarted)
                throw new ArgumentException("Robot is not in a state for mounting components");

            var components =
                await RobotComponentsRepository.GetRobotBodyComponentsByRobotIdAndComponentTypeAsync(robotObject.Id);
            if (components == null || !components.Any())
                throw new DataException("There is not any maching component in storage");
            if (components.Count != 1)
                throw new DataException("There is conflict in the components. More than one body found for robot with id " + robotObject.Id);
            var bodyToMount = components.Single();

            await RobotRepository.AddRobotComponentAsync(robotObject.Id, typeof(Body), bodyToMount);
            await RobotRepository.UpdateRobotProperty(robotObject.Id, robot => robot.Body.MountedAt,
                DateTime.Now);
            await RobotComponentsRepository.DeleteRobotComponentAsync(bodyToMount.Id);

            var newMessageModel = new RobotConstructionMountHeadMessage()
            {
                RobotId = robotObject.Id
            };

            return JsonSerializer.Serialize(newMessageModel);
        }
    }
}
