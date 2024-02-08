using Microsoft.Extensions.Options;
using RobotFactory.DataAccessLayer.QueueServices.BaseModels;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.ConstructionOrganizers;
using RobotFactory.Workers.ConstructionOrganizers.Helpers;
using RobotFactory.Workers.ConstructionOrganizers.QueueServices;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<QueueServicesConfiguration>(builder.Configuration.GetSection("QueueServiceConfig"));

//Register repositories
builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();

//Register Services
builder.Services.AddSingleton<IBaseWorkerQueueConsumer<StartRobotConstructionMessage>>(provider =>
{
    var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
    var logger = provider.GetRequiredService<ILogger<BaseWorkerQueueConsumer<StartRobotConstructionMessage>>>();

    return new StartRobotConstructionQueueConsumerService(
        logger,
        config.StartConstructionQueueName,
        config.QueueServiceUri,
        config.StartConstructionQueueSasToken
    );
});
builder.Services.AddSingleton<IBaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>>(provider =>
{
    var config = provider.GetRequiredService<IOptions<QueueServicesConfiguration>>().Value;
    var logger = provider.GetRequiredService<ILogger<BaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>>>();

    return new BaseWorkerQueuePublisher<RobotConstructionMountBodyMessage>(
        config.MountBodyQueueName,
        config.QueueServiceUri,
        config.MountBodyQueueSasToken,
        logger);
});

//Register Workers
builder.Services.AddHostedService<StartRobotConstructionWorker>();

var host = builder.Build();
host.Run();
