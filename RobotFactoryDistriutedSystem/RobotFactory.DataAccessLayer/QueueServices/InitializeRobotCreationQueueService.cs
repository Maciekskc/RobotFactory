using Azure;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public class InitializeRobotCreationQueueService : IInitializeRobotCreationQueueService
    {
        private readonly ILogger<InitializeRobotCreationQueueService> _loggger;
        private QueueClient _queueClient = null;

        public InitializeRobotCreationQueueService(IConfiguration configuration, ILogger<InitializeRobotCreationQueueService> loggger)
        {
            _loggger = loggger;
            string queueName = configuration["AzureStorageQueue:InitializeRobotCreationQueueName"];
            string queueUri = configuration["AzureStorageQueue:QueueUri"];
            string sasSignature = configuration["AzureStorageQueue:SASTokenConnection"];

            // Instantiate a QueueClient to create and interact with the queue
            _queueClient = new QueueClient(
                new Uri(queueUri+queueName),
                new AzureSasCredential(sasSignature));
        }

        public async Task AddMessageToQueue(InitializeRobotCreation message)
        {
            if (_queueClient == null)
                throw new ArgumentNullException("QueueClient cannot be initialized");
            _loggger.LogInformation("Attempt to add initialize robot creation message to queue");
            var queueResponse = await _queueClient.SendMessageAsync(JsonConvert.SerializeObject(message));

            _loggger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);

        }
    }
}
