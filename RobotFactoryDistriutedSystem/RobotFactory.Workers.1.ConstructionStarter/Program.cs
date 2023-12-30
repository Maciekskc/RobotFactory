using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.ConstructionOrganizer;
using RobotFactory.Workers.ConstructionOrganizer.QueueServices;


var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<StartRobotConstruction>,
        StartRobotConstructionQueueConsumerService>();
builder.Services.AddSingleton<IBaseWorkerQueuePublisher,RobotBodyConstructionQueueProducerService>();

builder.Services.AddHostedService<StartRobotConstructionWorker>();
var host = builder.Build();
host.Run();
