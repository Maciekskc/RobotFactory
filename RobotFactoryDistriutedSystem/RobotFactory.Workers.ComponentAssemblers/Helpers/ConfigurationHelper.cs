using Microsoft.Extensions.Options;
using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.ComponentAssemblers.QueueServices.Arms;
using RobotFactory.Workers.ComponentAssemblers.QueueServices.Body;
using RobotFactory.Workers.ComponentAssemblers.QueueServices.Head;
using RobotFactory.Workers.ComponentAssemblers.QueueServices.Legs;
using RobotFactory.Workers.ComponentAssemblers.Workers;

namespace RobotFactory.Workers.ComponentAssemblers.Helpers
{
    public static class ConfigurationHelper
    {
        public static void RegisterQueueConsumers(IServiceCollection builderServices)
        {
            builderServices.AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>>>();

                return new RobotBodyConstructionQueueConsumerService(
                    logger,
                    config.MountBodyQueueName,
                    config.QueueServiceUri,
                    config.MountBodyQueueSasToken
                    );
            });

            builderServices.AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>>>();

                return new RobotHeadConstructionQueueConsumerService(
                    logger,
                    config.MountHeadQueueName,
                    config.QueueServiceUri,
                    config.MountHeadQueueSasToken
                );
            });

            builderServices.AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>>>();

                return new RobotArmsConstructionQueueConsumerService(
                    logger,
                    config.MountArmsQueueName,
                    config.QueueServiceUri,
                    config.MountArmsQueueSasToken
                );
            });

            builderServices.AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>>>();

                return new RobotLegsConstructionQueueConsumerService(
                    logger,
                    config.MountLegsQueueName,
                    config.QueueServiceUri,
                    config.MountLegsQueueSasToken
                );
            });
        }

        public static void RegisterQueuePublishers(IServiceCollection builderServices)
        {

            builderServices.AddSingleton<IBaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>>>();

                return new BaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>(
                    config.MountBodyQueueName,
                    config.QueueServiceUri,
                    config.MountBodyQueueSasToken,
                    logger);
            });

            builderServices.AddSingleton<IBaseWorkerQueuePublisher<RobotConstructionMountHeadMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueuePublisher<RobotConstructionMountHeadMessage>>>();

                return new BaseWorkerQueuePublisher<RobotConstructionMountHeadMessage>(
                    config.MountHeadQueueName,
                    config.QueueServiceUri,
                    config.MountHeadQueueSasToken,
                    logger);
            });

            builderServices.AddSingleton<IBaseWorkerQueuePublisher<RobotConstructionMountArmsMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueuePublisher<RobotConstructionMountArmsMessage>>>();

                return new BaseWorkerQueuePublisher<RobotConstructionMountArmsMessage>(
                    config.MountArmsQueueName,
                    config.QueueServiceUri,
                    config.MountArmsQueueSasToken,
                    logger);
            });

            builderServices.AddSingleton<IBaseWorkerQueuePublisher<RobotConstructionMountLegsMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueuePublisher<RobotConstructionMountLegsMessage>>>();

                return new BaseWorkerQueuePublisher<RobotConstructionMountLegsMessage>(
                    config.MountLegsQueueName,
                    config.QueueServiceUri,
                    config.MountLegsQueueSasToken,
                    logger);
            });

            builderServices.AddSingleton<IBaseWorkerQueuePublisher<FinalizeRobotConstructionMessage>>(provider =>
            {
                var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
                var logger = provider.GetRequiredService<ILogger<BaseWorkerQueuePublisher<FinalizeRobotConstructionMessage>>>();

                return new BaseWorkerQueuePublisher<FinalizeRobotConstructionMessage>(
                    config.FinalizeConstructionQueueName,
                    config.QueueServiceUri,
                    config.FinalizeConstructionQueueSasToken,
                    logger);
            });
        }
        

        public static void RegisterWorkers(IServiceCollection builderServices)
        {
            builderServices.AddHostedService<RobotConstructionMountBodyWorker>();
            builderServices.AddHostedService<RobotConstructionMountHeadWorker>();
            builderServices.AddHostedService<RobotConstructionMountArmsWorker>();
            builderServices.AddHostedService<RobotConstructionMountLegsWorker>();
        }
    }
}
