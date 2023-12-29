using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.ConstructionOrganizer;
using RobotFactory.Workers.ConstructionOrganizer.QueueServices;
using RobotFactory.Workers.SharedComponents.QueueClientInterfaces;
using RobotFactory.Workers.SharedComponents.QueueClientServiceInterfaces;


var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<StartRobotConstruction>,
        StartRobotConstructionQueueConsumerService>();
builder.Services.AddSingleton<IBaseWorkerQueuePublisher,RobotBodyConstructionQueueProducerService>();

builder.Services.AddHostedService<StartRobotConstructionWorker>();
var host = builder.Build();
host.Run();
