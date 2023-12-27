using System.Text.Json;
using Azure;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public class StartRobotConstructionService : BaseQueueService, IStartRobotConstructionService
    {
        private readonly ILogger<StartRobotConstructionService> _loggger;

        public StartRobotConstructionService(IConfiguration configuration, ILogger<StartRobotConstructionService> loggger)
        :base(configuration, "AzureStorageQueue:StartRobotConstructionQueueName", "AzureStorageQueue:QueueBaseUri", "AzureStorageQueue:StartRobotConstructionQueueSasToken")
        {
            _loggger = loggger;
        }

        public async Task AddMessageToQueue(StartRobotConstruction robotConstructionRequest)
        {
            if (_queueClient == null)
                throw new ArgumentNullException("QueueClient cannot be initialized");
            _loggger.LogInformation("Attempt to add start robot construction message to queue");
            var queueResponse = await _queueClient.SendMessageAsync(JsonSerializer.Serialize(robotConstructionRequest));

            _loggger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);
        }
    }
}