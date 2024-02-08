using Azure.Storage.Queues;
using Azure;

namespace RobotFactory.DataAccessLayer.QueueServices.BaseModels
{
    public abstract class BaseQueueService
    {
        protected readonly QueueClient? QueueClient = null;

        public BaseQueueService(string queueName, string queueUri, string queueSasToken)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentNullException($"Miss queue name setting. QueueClient cannot be initialized");
            }
            if (string.IsNullOrEmpty(queueUri))
            {
                throw new ArgumentNullException($"Miss queue uri setting. QueueClient cannot be initialized");
            }
            if (string.IsNullOrEmpty(queueSasToken))
            {
                throw new ArgumentNullException($"Miss queue sas token setting. QueueClient cannot be initialized");
            }

            // Instantiate a QueueClient to create and interact with the queue
            QueueClient = new QueueClient(
                new Uri(queueUri + queueName),
                new AzureSasCredential(queueSasToken));
        }

        protected bool IsQueueInitialized() => QueueClient != null;
    }
}
