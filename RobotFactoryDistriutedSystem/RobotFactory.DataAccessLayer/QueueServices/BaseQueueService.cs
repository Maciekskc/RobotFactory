using Azure.Storage.Queues;
using Azure;
using Microsoft.Extensions.Configuration;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public abstract class BaseQueueService
    {
        protected readonly QueueClient _queueClient = null;

        public BaseQueueService(IConfiguration configuration, string QueueName, string QueueUri, string SasSignature)
        {
            string queueName = configuration[QueueName] ?? throw new ArgumentNullException("Configuration  cannot be loaded.");
            string queueUri = configuration[QueueUri] ?? throw new ArgumentNullException("Configuration  cannot be loaded.");
            string sasSignature = configuration[SasSignature] ?? throw new ArgumentNullException("Configuration  cannot be loaded.");

            // Instantiate a QueueClient to create and interact with the queue
            _queueClient = new QueueClient(
                new Uri(queueUri + queueName),
                new AzureSasCredential(sasSignature));
        }
    }
}
