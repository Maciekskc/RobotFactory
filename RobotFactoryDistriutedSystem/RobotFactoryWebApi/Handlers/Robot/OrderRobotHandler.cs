using MediatR;
using Newtonsoft.Json;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.OrderRobots;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.WebApi.Handlers.Robot
{
    public class OrderRobotHandler : IRequestHandler<OrderRobotRequest, OrderRobotResponse>
    {
        private readonly ILogger<OrderRobotHandler> _logger;
        private readonly IRobotRepository _robotRepository;
        private readonly IInitializeRobotCreationQueueService _initializeRobotCreationQueueService;

        public OrderRobotHandler(ILogger<OrderRobotHandler> logger, IRobotRepository robotRepository, IInitializeRobotCreationQueueService initializeRobotCreationQueueService)
        {
            _logger = logger;
            _robotRepository = robotRepository;
            _initializeRobotCreationQueueService = initializeRobotCreationQueueService;
        }

        public async Task<OrderRobotResponse> Handle(OrderRobotRequest request, CancellationToken cancellationToken)
        {
            //Validate request
            var robot = await AddNewRobotToMongoDatabse();

            _logger.LogInformation("Robot added to the daabase. Creating Message to construct");
            await AddMessageToQueue(robot.Id);
            return new OrderRobotResponse(robot.Id);
        }

        private async Task<DataLayer.Models.Robot> AddNewRobotToMongoDatabse()
        {
            var robot = new DataLayer.Models.Robot()
            {
                OrderedAt = DateTime.Now
            };
            _logger.LogInformation("Attempt to create robot in the database. Robot: {0}", JsonConvert.SerializeObject(robot));
            await _robotRepository.CreateRobotAsync(robot);

            return robot;
        }


        private async Task AddMessageToQueue(string Id)
        {
            var message = new InitializeRobotCreation()
            {
                RobotId = Id,
                Issuer = "HardcodedInWebApiApplication",
                OrderElements = new RobotComponentsOrder()
                {
                    Items = new RobotComponentOrderItem[]
                    {
                        new RobotComponentOrderItem(RobotComponentType.Body,new []{"2","2"}),
                        new RobotComponentOrderItem(RobotComponentType.Head,new []{"8"}),
                        new RobotComponentOrderItem(RobotComponentType.Arm,new []{"Left"}),
                        new RobotComponentOrderItem(RobotComponentType.Arm,new []{"Right"}),
                        new RobotComponentOrderItem(RobotComponentType.Leg,new []{"Left"}),
                        new RobotComponentOrderItem(RobotComponentType.Leg,new []{"Right"})
                    }
                }
            };

            await _initializeRobotCreationQueueService.AddMessageToQueue(message);
        }
    }
}
