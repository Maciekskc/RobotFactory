using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.Workers.SharedComponents
{
    public class BaseRobotConstructionWorker<QcType,QpType> : BackgroundService 
        where QcType : class 
        where QpType : class
    {

        private const string ParallelMessageProcessingCountConfigurationName = "WorkerConfiguration:WorkerParallelMessageProcessingCount";
        private const string QueueReadDelaySettingName = "WorkerConfiguration:WorkerProessingDelay";

        private readonly ILogger<BaseRobotConstructionWorker<QcType, QpType>> _logger;
        private readonly int _parallelMessageProcessingCount;
        private readonly int _queueReadDelay;

        protected IBaseWorkerQueueConsumer<QcType> QueueConsumer { get; set; }
        protected IBaseWorkerQueuePublisher<QpType> QueuePublisher { get; set; }
        protected IRobotComponentsRepository RobotComponentsRepository { get; set; }
        protected IRobotRepository RobotRepository { get; set; }

        public BaseRobotConstructionWorker(ILogger<BaseRobotConstructionWorker<QcType, QpType>> logger, IBaseWorkerQueueConsumer<QcType> queueConsumer, IBaseWorkerQueuePublisher<QpType> queuePublisher, IConfiguration configuration, IRobotComponentsRepository robotComponentsRepository, IRobotRepository robotRepository)
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
                List<QueueMessageWrapper<QcType>> messagesList = new List<QueueMessageWrapper<QcType>>();
                try
                {
                    messagesList = await QueueConsumer.ReadMessagesAsync(_parallelMessageProcessingCount);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Cannot read messages from queue. Exception thrown: {0}", ex.Message);
                }

                foreach (var message in messagesList)
                {
                    try
                    {
                        _logger.LogInformation("Processing next message from queue");
                        var processedMessage = await ExecuteQueueActionAsync(message.MessageObject);
                        await QueuePublisher.PublishMessageAsync(processedMessage);
                        await QueueConsumer.RemoveMessagesFromQueueAsync(message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Cannot process message. Message ID {0}. Exception {1}", message.GetType().GetProperty("RobotId"), ex);
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
        protected virtual Task<string> ExecuteQueueActionAsync(QcType message)
        {
            throw new NotImplementedException();
        }
    }
}