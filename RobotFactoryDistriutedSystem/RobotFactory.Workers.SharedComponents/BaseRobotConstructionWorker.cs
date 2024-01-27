using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;

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
        protected IRobotComponentsRepository RobotComponentsRepository { get; set; }
        protected IRobotRepository RobotRepository { get; set; }

        public BaseRobotConstructionWorker(ILogger<BaseRobotConstructionWorker<T>> logger, IBaseWorkerQueueConsumer<T> queueConsumer, IBaseWorkerQueuePublisher queuePublisher, IConfiguration configuration, IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository)
        {
            _logger = logger;
            QueueConsumer = queueConsumer;
            QueuePublisher = queuePublisher;
            RobotComponentsRepository = robotComponentsRepository;
            RobotRepository = robotRepository;
            if (!Int32.TryParse(configuration[ParallelMessageProcessingCountConfigurationName], out _parallelMessageProcessingCount))
                _parallelMessageProcessingCount = 1;
            if (!Int32.TryParse(configuration[QueueReadDelaySettingName], out _queueReadDelay))
                _queueReadDelay = 1000;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                List<T> messagesList = new List<T>();
                try
                {
                    messagesList = await QueueConsumer.ReadMessagesAsync(_parallelMessageProcessingCount);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Cannot read messages from queue. Exception thrown: {0}", ex.Message);
                    throw;
                }

                foreach (var message in messagesList)
                {
                    try
                    {
                        _logger.LogInformation("Processing next message from queue");
                        var processedMessage = await ExecuteQueueActionAsync(message);
                        await QueuePublisher.PublishMessageAsync(processedMessage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Cannot process message. Message ID {0}. Exception {1}", message.GetType().GetProperty("RobotId"), ex);
                        throw;
                    }
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
        protected virtual Task<string> ExecuteQueueActionAsync(T message)
        {
            throw new NotImplementedException();
        }
    }
}