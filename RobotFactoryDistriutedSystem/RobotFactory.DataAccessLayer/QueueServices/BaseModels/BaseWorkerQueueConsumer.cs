using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices.BaseModels
{
    public class BaseWorkerQueueConsumer<T> : BaseQueueService, IBaseWorkerQueueConsumer<T> where T : class
    {

        private readonly ILogger<BaseWorkerQueueConsumer<T>> _logger;

        public BaseWorkerQueueConsumer(ILogger<BaseWorkerQueueConsumer<T>> logger, string queueName, string queueUri, string queueSasToken)
            : base(queueName, queueUri, queueSasToken)
        {
            _logger = logger;
        }

        public async Task<List<QueueMessageWrapper<T>>> ReadMessagesAsync(int maxMessageCount)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient was not initialized");
            _logger.LogDebug("Attempt to read messages from the queue");

            QueueMessage[] receivedMessages = await QueueClient.ReceiveMessagesAsync(maxMessageCount);

            _logger.LogDebug("Found {0} messages", receivedMessages.Length);

            List<QueueMessageWrapper<T>> deconstructedMessages = new List<QueueMessageWrapper<T>>();
            foreach (QueueMessage message in receivedMessages)
                try
                {
                    _logger.LogDebug("Deserializing message: {0} into object of type {1}", message.MessageText, typeof(T));
                    deconstructedMessages.Add(
                        new QueueMessageWrapper<T>()
                        {
                            MessageId = message.MessageId,
                            MessagePopReceipt = message.PopReceipt,
                            MessageObject = JsonSerializer.Deserialize<T>(message.MessageText)
                        });
                }
                catch (Exception ex)
                {
                    _logger.LogError("Cannot deserialize message with Id: {0}", message.MessageId);
                    throw;
                }

            return deconstructedMessages;
        }

        public async Task RemoveMessagesFromQueueAsync(QueueMessageWrapper<T> message)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient was not initialized");
            await QueueClient.DeleteMessageAsync(message.MessageId, message.MessagePopReceipt);
            _logger.LogDebug("Message {0} consumed from reading queue", message.MessageId);
        }
    }
}
