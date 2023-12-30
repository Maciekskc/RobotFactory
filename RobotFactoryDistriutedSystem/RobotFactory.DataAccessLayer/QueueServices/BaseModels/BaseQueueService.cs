using Azure.Storage.Queues;
using Azure;
using Microsoft.Extensions.Configuration;

namespace RobotFactory.DataAccessLayer.QueueServices.BaseModels
{
    public abstract class BaseQueueService
    {
        protected readonly QueueClient? QueueClient = null;

        public BaseQueueService(IConfiguration configuration, string queueNameSettingName, string queueUriSettingName, string queueSasSignatureSettingName)
        {
            string queueName = configuration[queueNameSettingName] ?? throw new ArgumentNullException($"Miss queue name setting: {queueNameSettingName}. QueueClient cannot be initialized");
            string queueUri = configuration[queueUriSettingName] ?? throw new ArgumentNullException($"Miss queue uri setting: {queueUriSettingName}. QueueClient cannot be initialized");
            string sasSignature = configuration[queueSasSignatureSettingName] ?? throw new ArgumentNullException($"Miss queue sas token setting: {queueSasSignatureSettingName}. QueueClient cannot be initialized");

            // Instantiate a QueueClient to create and interact with the queue
            QueueClient = new QueueClient(
                new Uri(queueUri + queueName),
                new AzureSasCredential(sasSignature));
        }

        protected bool IsQueueInitialized() => QueueClient != null;
    }
}
