using RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.Workers._2.MountLegsWorker.QueueServices;
using RobotFactory.Workers._5.MountLegs;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddSingleton<IBaseWorkerQueueConsumer<RobotConstructionMountLegsMessage>,
        RobotLegsConstructionQueueConsumerService>();
builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();
builder.Services.AddHostedService<RobotConstructionMountLegsWorker>();

var host = builder.Build();
host.Run();
