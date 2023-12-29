using Azure;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public class InitializeRobotCreationQueueService : BaseQueueService, IInitializeRobotCreationQueueService
    {
        private readonly ILogger<InitializeRobotCreationQueueService> _loggger;

        public InitializeRobotCreationQueueService(IConfiguration configuration, ILogger<InitializeRobotCreationQueueService> loggger)
        :base(configuration, "AzureStorageQueue:InitializeRobotCreationQueueName", "AzureStorageQueue:QueueBaseUri", "AzureStorageQueue:InitializeRobotCreationQueueSasToken")
        {
            _loggger = loggger;
        }

        public async Task AddMessageToQueue(InitializeRobotCreation message)
        {
            if (QueueClient == null)
                throw new ArgumentNullException("QueueClient cannot be initialized");
            _loggger.LogInformation("Attempt to add initialize robot creation message to queue");
            var queueResponse = await QueueClient.SendMessageAsync(JsonConvert.SerializeObject(message));

            _loggger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);

        }
    }
}
