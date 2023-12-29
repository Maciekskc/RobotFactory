using Microsoft.Extensions.Configuration;
using RobotFactory.Workers.SharedComponents.QueueClientInterfaces;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices;

namespace RobotFactory.Workers.SharedComponents.QueueClientServices
{
    public class BaseWorkerQueuePublisher : BaseQueueService, IBaseWorkerQueuePublisher
    {
        private const string QueueNameSettingName = "WorkerConfiguration:WorkerProducingMessageQueueName";
        private const string QueueUriSettingName = "WorkerConfiguration:WorkerProducingMessageQueueUri";
        private const string QueueSasTokenSettngName = "WorkerConfiguration:WorkerProducingMessageQueueSasToken";

        private readonly ILogger<BaseWorkerQueuePublisher> _logger;
        public BaseWorkerQueuePublisher(IConfiguration configuration, ILogger<BaseWorkerQueuePublisher> logger)
            : base(configuration, QueueNameSettingName, QueueUriSettingName, QueueSasTokenSettngName)
        {
            _logger = logger;
        }

        public async Task PublishMessageAsync(string message)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient was not initialized");
            _logger.LogInformation("Attempt to add initialize robot creation message to queue");
            var queueResponse = await QueueClient.SendMessageAsync(message);

            _logger.LogInformation("Message Added. New Mesage Id: {0}", queueResponse.Value.MessageId);
        }
    }
}
