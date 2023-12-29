using Azure.Storage.Queues;
using Azure;
using Microsoft.Extensions.Configuration;

namespace RobotFactory.DataAccessLayer.QueueServices
{
    public abstract class BaseQueueService
    {
        protected readonly QueueClient? QueueClient = null;

        public BaseQueueService(IConfiguration configuration, string queueNameSettingName, string queueUriSettingName, string queueSasSignatureSettingName)
        {
            string queueName = configuration[queueNameSettingName] ?? throw new ArgumentNullException("Configuration  cannot be loaded.");
            string queueUri = configuration[queueUriSettingName] ?? throw new ArgumentNullException("Configuration  cannot be loaded.");
            string sasSignature = configuration[queueSasSignatureSettingName] ?? throw new ArgumentNullException("Configuration  cannot be loaded.");

            // Instantiate a QueueClient to create and interact with the queue
            QueueClient = new QueueClient(
                new Uri(queueUri + queueName),
                new AzureSasCredential(sasSignature));
        }

        protected bool IsQueueInitialized() => QueueClient != null;
    }
}
