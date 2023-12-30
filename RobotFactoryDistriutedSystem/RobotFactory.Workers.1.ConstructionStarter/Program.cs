using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers.ConstructionOrganizer;
using RobotFactory.Workers.ConstructionOrganizer.QueueServices;


var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<StartRobotConstruction>,
        StartRobotConstructionQueueConsumerService>();
builder.Services.AddSingleton<IBaseWorkerQueuePublisher,RobotBodyConstructionQueueProducerService>();
builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();

builder.Services.AddHostedService<StartRobotConstructionWorker>();
var host = builder.Build();
host.Run();
