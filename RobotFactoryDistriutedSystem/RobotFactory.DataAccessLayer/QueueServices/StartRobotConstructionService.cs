using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public class StartRobotConstructionService : BaseQueueService, IStartRobotConstructionService
    {
        private const string QueueNameSettingName = "AzureStorageQueue:StartRobotConstructionQueueName";
        private const string QueueUriSettingName = "AzureStorageQueue:QueueBaseUri";
        private const string QueueSasTokenSettingName = "AzureStorageQueue:StartRobotConstructionQueueSasToken";

        private readonly ILogger<StartRobotConstructionService> _logger;

        public StartRobotConstructionService(IConfiguration configuration, ILogger<StartRobotConstructionService> logger)
            : base(configuration, QueueNameSettingName, QueueUriSettingName, QueueSasTokenSettingName)
        {
            _logger = logger;
        }

        public async Task AddMessageToQueue(StartRobotConstruction robotConstructionRequest)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient cannot be initialized");
            _logger.LogInformation("Attempt to add start robot construction message to queue");
            var queueResponse = await QueueClient.SendMessageAsync(JsonSerializer.Serialize(robotConstructionRequest));

            _logger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);
        }
    }
}