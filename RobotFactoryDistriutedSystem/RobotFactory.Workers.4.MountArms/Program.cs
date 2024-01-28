using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers._4.MountArms;
using RobotFactory.Workers._4.MountArms.QueueServices;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountArmsMessage>,
        RobotArmsConstructionQueueConsumerService>();
builder.Services.AddSingleton<IBaseWorkerQueuePublisher, RobotLegsConstructionQueueProducerService>();
builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();
builder.Services.AddHostedService<RobotConstructionMountArmsWorker>();

var host = builder.Build();
host.Run();
