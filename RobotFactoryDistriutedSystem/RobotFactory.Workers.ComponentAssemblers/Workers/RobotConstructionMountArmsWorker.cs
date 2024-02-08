using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using System.Data;
using System.Text.Json;
using RobotFactory.Workers.SharedComponents;

namespace RobotFactory.Workers.ComponentAssemblers.Workers
{
    public class RobotConstructionMountArmsWorker : BaseRobotConstructionWorker<RobotConstructionMountArmsMessage,RobotConstructionMountLegsMessage>
    {
        private readonly ILogger<RobotConstructionMountArmsWorker> _logger;

        public RobotConstructionMountArmsWorker(
            ILogger<BaseRobotConstructionWorker<RobotConstructionMountArmsMessage, RobotConstructionMountLegsMessage>> logger,
            IBaseWorkerQueueConsumer<RobotConstructionMountArmsMessage> queueConsumer,
            IBaseWorkerQueuePublisher<RobotConstructionMountLegsMessage> queuePublisher, IConfiguration configuration,
            IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository,
            ILogger<RobotConstructionMountArmsWorker> logger1) : base(logger, queueConsumer, queuePublisher,
            configuration, robotComponentsRepository, robotRepository)
        {
            _logger = logger1;
        }

        protected override async Task<string> ExecuteQueueActionAsync(RobotConstructionMountArmsMessage message)
        {
            _logger.LogInformation("{0} is processing the request", nameof(RobotConstructionMountArmsWorker));

            var robotObject = await RobotRepository.GetRobotByIdAsync(message.RobotId);
            if (robotObject == null)
                throw new ArgumentException("Robot with given Id does not exist");

            if (robotObject.ConstructionStatus != RobotConstrucionStatus.ConstructionStarted)
                throw new ArgumentException("Robot is not in a state for mounting components");

            var components =
                await RobotComponentsRepository.GetRobotArmsComponentsByRobotIdAndComponentTypeAsync(robotObject.Id);
            if (components == null || !components.Any())
                throw new DataException("There is not any maching component in storage");
            if (robotObject.Body == null || robotObject.Body.ArmsNumbers != components.Count)
                throw new DataException("Number of arms from storage does not match robot body arm slots. Robot with id " + robotObject.Id);
            foreach (var arm in components)
            {
                arm.MountedAt = DateTime.Now;
                await RobotRepository.AddRobotComponentAsync(robotObject.Id, typeof(Arm), arm);
                await RobotComponentsRepository.DeleteRobotComponentAsync(arm.Id);
            }

            var newMessageModel = new RobotConstructionMountHeadMessage()
            {
                RobotId = robotObject.Id
            };

            return JsonSerializer.Serialize(newMessageModel);
        }

        
    }
}
