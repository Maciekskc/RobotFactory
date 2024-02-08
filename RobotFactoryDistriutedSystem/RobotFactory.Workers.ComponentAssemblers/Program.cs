using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.Workers.ComponentAssemblers.Helpers;

var builder = Host.CreateApplicationBuilder(args);


//Write configuration class model with devfault properties and read properties from datamodel
builder.Services.Configure<QueueServicesConfiguration>(builder.Configuration.GetSection("QueueServiceConfig"));

ConfigurationHelper.RegisterQueueConsumers(builder.Services);
ConfigurationHelper.RegisterQueuePublishers(builder.Services);
ConfigurationHelper.RegisterWorkers(builder.Services);

builder.Services.AddSingleton<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddSingleton<IRobotRepository, RobotRepository>();
var host = builder.Build();
host.Run();
