using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public class InitializeRobotCreationQueueService : BaseQueueService, IInitializeRobotCreationQueueService
    {
        private const string QueueNameSettingName = "AzureStorageQueue:InitializeRobotCreationQueueName";
        private const string QueueUriSettingName = "AzureStorageQueue:QueueBaseUri";
        private const string QueueSasTokenSettingName = "AzureStorageQueue:InitializeRobotCreationQueueSasToken";

        private readonly ILogger<InitializeRobotCreationQueueService> _logger;

        public InitializeRobotCreationQueueService(IConfiguration configuration, ILogger<InitializeRobotCreationQueueService> logger)
            : base(configuration[QueueNameSettingName], configuration[QueueUriSettingName], configuration[QueueSasTokenSettingName])
        {
            _logger = logger;
        }

        public async Task AddMessageToQueue(InitializeRobotCreation message)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient cannot be initialized");
            _logger.LogInformation("Attempt to add initialize robot creation message to queue");
            var queueResponse = await QueueClient.SendMessageAsync(JsonConvert.SerializeObject(message));

            _logger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);

        }
    }
}
