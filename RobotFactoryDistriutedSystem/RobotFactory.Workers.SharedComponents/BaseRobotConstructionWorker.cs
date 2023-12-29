using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RobotFactory.Workers.SharedComponents.QueueClientInterfaces;
using RobotFactory.Workers.SharedComponents.QueueClientServiceInterfaces;

namespace RobotFactory.Workers.SharedComponents
{
    public class BaseRobotConstructionWorker<T> : BackgroundService where T : class
    {

        private const string ParallelMessageProcessingCountConfigurationName = "WorkerConfiguration:WorkerParallelMessageProcessingCount";
        private const string QueueReadDelaySettingName = "WorkerConfiguration:WorkerProessingDelay";

        private readonly ILogger<BaseRobotConstructionWorker<T>> _logger;
        private readonly int _parallelMessageProcessingCount;
        private readonly int _queueReadDelay;

        protected IBaseWorkerQueueConsumer<T> QueueConsumer { get; set; }
        protected IBaseWorkerQueuePublisher QueuePublisher { get; set; }

        public BaseRobotConstructionWorker(ILogger<BaseRobotConstructionWorker<T>> logger, IBaseWorkerQueueConsumer<T> queueConsumer, IBaseWorkerQueuePublisher queuePublisher, IConfiguration configuration)
        {
            _logger = logger;
            QueueConsumer = queueConsumer;
            QueuePublisher = queuePublisher;
            if (!Int32.TryParse(configuration[ParallelMessageProcessingCountConfigurationName], out _parallelMessageProcessingCount))
                _parallelMessageProcessingCount = 1;
            if (!Int32.TryParse(configuration[QueueReadDelaySettingName], out _queueReadDelay))
                _queueReadDelay = 1000;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var messages = await QueueConsumer.ReadMessagesAsync(_parallelMessageProcessingCount);

                foreach (var message in messages)
                {
                    _logger.LogInformation("Processing next message from queue");
                    var processedMessage = await ExecuteQueueActionAsync(message);
                    await QueuePublisher.PublishMessageAsync(processedMessage);
                }
                
                await Task.Delay(_queueReadDelay, stoppingToken);
            }
        }

        /// <summary>
        /// Service action message
        /// </summary>
        /// <param name="message">Single message from processing queue, deserialized into object</param>
        /// <returns>Queue Message object serialized into string</returns>
        /// <exception cref="NotImplementedException"></exception>
        protected virtual async Task<string> ExecuteQueueActionAsync(T message)
        {
            throw new NotImplementedException();
        }
    }
}