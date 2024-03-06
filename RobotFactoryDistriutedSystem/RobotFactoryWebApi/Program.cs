using Azure.Identity;
using MediatR;
using RobotFactory.DataAccessLayer.QueueServices;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.OrderRobots;
using RobotFactorySharedComponents.Dtos.ApiRequests.HealthCheck;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.SupplyComponents;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(
            builder.Configuration["AzureKeyVaultConnection"])
        .ConfigureKeyVault(kv =>
        {
            kv.SetCredential(new DefaultAzureCredential());
        });
});

// Add services to the container.
builder.Services.AddScoped<IRobotRepository, RobotRepository>();
builder.Services.AddScoped<IRobotComponentsRepository, RobotComponentsRepository>();
builder.Services.AddScoped<IInitializeRobotCreationQueueService, InitializeRobotCreationQueueService>();
builder.Services.AddScoped<IStartRobotConstructionService, StartRobotConstructionService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var mediator = app.Services.CreateScope().ServiceProvider.GetService<IMediator>();

app.MapGet("/hello", () => builder.Configuration["ServiceResponse"]);
app.MapGet("/health-check", () => mediator.Send(new HealthCheckRequest()));
app.MapPost("/order-robot", () => mediator.Send(new OrderRobotRequest()));
app.MapPost("/supply-components",  (SupplyComponentsRequest request) => mediator.Send(request));

app.Run();
