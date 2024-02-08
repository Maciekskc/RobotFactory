using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers._2.MountBodyWorker;
using RobotFactory.Workers._2.MountBodyWorker.QueueServices;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountBodyMessage>,
        RobotBodyConstructionQueueConsumerService>();
builder.Services.AddSingleton<IBaseWorkerQueuePublisher, RobotHeadConstructionQueueProducerService>();
builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();

builder.Services.AddHostedService<RobotConstructionMountBodyWorker>();
var host = builder.Build();
host.Run();
