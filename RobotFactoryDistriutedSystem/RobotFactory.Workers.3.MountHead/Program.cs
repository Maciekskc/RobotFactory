using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers._3.MountHead;
using RobotFactory.Workers._3.MountHead.QueueServices;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountHeadMessage>,
        RobotHeadConstructionQueueConsumerService>();
builder.Services.AddSingleton<IBaseWorkerQueuePublisher, RobotArmsConstructionQueueProducerService>();
builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();
builder.Services.AddHostedService<RobotConstructionMountHeadWorker>();

var host = builder.Build();
host.Run();
