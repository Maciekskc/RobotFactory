using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public class InitializeRobotCreationQueueService : IInitializeRobotCreationQueueService
    {
        private QueueClient _queueClient = null;

        public InitializeRobotCreationQueueService(IConfiguration configuration)
        {
            string queueName = configuration["AzureStorageQueue:InitializeRobotCreationQueueName"];
            string connectionString = configuration["AzureStorageQueue:ConnectionString"];

            // Instantiate a QueueClient to create and interact with the queue
            QueueClient queueClient = new QueueClient(connectionString,queueName);
        }

        public Task AddMessageToQueue(InitializeRobotCreation queue)
        {
            
        }
    }
}
