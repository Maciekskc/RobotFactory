using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using System.Data;
using System.Text.Json;
using RobotFactory.Workers.SharedComponents;

namespace RobotFactory.Workers._5.MountLegs
{
    public class RobotConstructionMountLegsWorker : BaseRobotConstructionWorker<RobotConstructionMountLegsMessage>
    {
        private readonly ILogger<RobotConstructionMountLegsWorker> _logger;

        public RobotConstructionMountLegsWorker(ILogger<BaseRobotConstructionWorker<RobotConstructionMountLegsMessage>> logger, IBaseWorkerQueueConsumer<RobotConstructionMountLegsMessage> queueConsumer, IBaseWorkerQueuePublisher queuePublisher, IConfiguration configuration, IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository, ILogger<RobotConstructionMountLegsWorker> logger1)
            : base(logger, queueConsumer, queuePublisher, configuration, robotComponentsRepository, robotRepository)
        {
            _logger = logger1;
        }

        protected override async Task<string> ExecuteQueueActionAsync(RobotConstructionMountLegsMessage message)
        {
            _logger.LogInformation("{0} is processing the request", nameof(RobotConstructionMountLegsWorker));

            var robotObject = await RobotRepository.GetRobotByIdAsync(message.RobotId);
            if (robotObject == null)
                throw new ArgumentException("Robot with given Id does not exist");

            if (robotObject.ConstructionStatus != RobotConstrucionStatus.ConstructionStarted)
                throw new ArgumentException("Robot is not in a state for mounting components");

            var components =
                await RobotComponentsRepository.GetRobotLegsComponentsByRobotIdAndComponentTypeAsync(robotObject.Id);
            if (components == null || !components.Any())
                throw new DataException("There is not any maching component in storage");
            if (robotObject.Body == null || robotObject.Body.ArmsNumbers != components.Count)
                throw new DataException("Number of legs from storage does not match robot body legs slots. Robot with id " + robotObject.Id);
            foreach (var leg in components)
            {
                leg.MountedAt = DateTime.Now;
                await RobotRepository.AddRobotComponentAsync(robotObject.Id, typeof(Leg), leg);
                await RobotComponentsRepository.DeleteRobotComponentAsync(leg.Id);
            }

            var newMessageModel = new RobotConstructionMountHeadMessage()
            {
                RobotId = robotObject.Id
            };

            return JsonSerializer.Serialize(newMessageModel);
        }
    }
}
