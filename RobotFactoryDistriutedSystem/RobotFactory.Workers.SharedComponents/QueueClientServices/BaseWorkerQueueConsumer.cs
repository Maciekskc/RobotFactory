﻿using System.Text;
using System.Text.Json;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices;
using RobotFactory.Workers.SharedComponents.QueueClientServiceInterfaces;

namespace RobotFactory.Workers.SharedComponents.QueueClientServices
{
    public class BaseWorkerQueueConsumer<T> : BaseQueueService, IBaseWorkerQueueConsumer<T> where T : class
    {
        private const string QueueNameSettingName = "WorkerConfiguration:WorkerConsumingMessageQueueName";
        private const string QueueUriSettingName = "WorkerConfiguration:WorkerConsumingMessageQueueUri";
        private const string QueueSasTokenSettingName = "WorkerConfiguration:WorkerConsumingMessageQueueSasToken";

        private readonly ILogger<BaseWorkerQueueConsumer<T>> _logger;

        public BaseWorkerQueueConsumer(IConfiguration configuration, ILogger<BaseWorkerQueueConsumer<T>> logger)
            : base(configuration, QueueNameSettingName, QueueUriSettingName, QueueSasTokenSettingName)

        {
            _logger = logger;
        }

        public async Task<List<T>> ReadMessagesAsync(int maxMessageCount)
        {
            if (!IsQueueInitialized())
                throw new ArgumentNullException("QueueClient was not initialized");
            _logger.LogDebug("Attempt to read messages from the queue");

            QueueMessage[] receivedMessages = await QueueClient.ReceiveMessagesAsync(maxMessageCount);

            _logger.LogDebug("Found {0} messages", receivedMessages.Length);

            List<T> deserializedMessages = new List<T>();
            foreach (QueueMessage message in receivedMessages)
                try
                {
                    var messageText = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
                    _logger.LogDebug("Deserializing message: {0} into object of type {1}", messageText, typeof(T));
                    deserializedMessages.Add(JsonSerializer.Deserialize<T>(messageText));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Cannot deserialize message with Id: {0}",message.MessageId);

                    //Here it should handle poisoned messages
                }

            return deserializedMessages;
        }
    }
}
