using System.Data;
using System.Text.Json;
using MongoDB.Bson.Serialization;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.ConstructionOrganizer.QueueServices;
using RobotFactory.Workers.SharedComponents;

namespace RobotFactory.Workers.ConstructionOrganizer
{
    public class StartRobotConstructionWorker : BaseRobotConstructionWorker<StartRobotConstruction>
    {
        private readonly ILogger<StartRobotConstructionWorker> _logger;
        public StartRobotConstructionWorker(ILogger<BaseRobotConstructionWorker<StartRobotConstruction>> logger, IBaseWorkerQueueConsumer<StartRobotConstruction> queueConsumer, IBaseWorkerQueuePublisher queuePublisher, IConfiguration configuration, IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository, ILogger<StartRobotConstructionWorker> logger1)
            : base(logger, queueConsumer, queuePublisher, configuration, robotComponentsRepository, robotRepository)
        {
            _logger = logger1;
        }

        protected override async Task<string> ExecuteQueueActionAsync(StartRobotConstruction message)
        {
            _logger.LogInformation("Worker is processing the request");

            var robotObject = await RobotRepository.GetRobotByIdAsync(message.RobotId);
            if (robotObject == null)
                throw new ArgumentException("Robot with given Id does not exist");

            //RobotRepository.UpdateRobotProperty(robotObject.Id, robot=>robot.ConstructionStatus, )
            //var components =
            //    await RobotComponentsRepository.GetRobotComponentsByRobotIdAndComponentTypeAsync(robotObject.Id,
            //        RobotComponentType.Body);
            //if(components == null || !components.Any())
            //    throw new DataException("There is not any maching component in storage");
            //if (components.Count != 1)
            //    throw new DataException("There is conflict in the components. More than one body found for robot with id " + robotObject.Id);

            //await RobotRepository.AddRobotComponentAsync(robotObject.Id, typeof(Body), components.Single());
            //await RobotComponentsRepository.DeleteRobotComponentAsync(components.Single().Id);

            //robotObject.ConstructionStatus = RobotConstrucionStatus.ConstructionStarted;
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
