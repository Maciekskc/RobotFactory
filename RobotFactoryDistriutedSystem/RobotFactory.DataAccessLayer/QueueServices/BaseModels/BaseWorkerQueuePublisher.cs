using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;

namespace RobotFactory.DataAccessLayer.QueueServices.BaseModels
{
    public class BaseWorkerQueuePublisher<T> : BaseQueueService, IBaseWorkerQueuePublisher<T> 
    {
        private readonly ILogger<BaseWorkerQueuePublisher<T>> _logger;
        public BaseWorkerQueuePublisher(string queueName, string queueUri, string queueSasToken, ILogger<BaseWorkerQueuePublisher<T>> logger)
            : base(queueName, queueUri, queueSasToken)
        {
            _logger = logger;
        }

        public async Task PublishMessageAsync(string message)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient was not initialized");
            _logger.LogInformation("Attempt to add message of type {0} to queue", typeof(T));
            var queueResponse = await QueueClient.SendMessageAsync(message);

            _logger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);
        }
    }
}
