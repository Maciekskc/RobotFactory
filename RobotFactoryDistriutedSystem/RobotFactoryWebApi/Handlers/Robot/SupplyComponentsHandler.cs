using MediatR;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.SupplyComponents;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.WebApi.Handlers.Robot
{
    public class SupplyComponentsHandler : IRequestHandler<SupplyComponentsRequest, SupplyComponentsResponse>
    {
        private readonly ILogger<OrderRobotHandler> _logger;
        private readonly IRobotComponentsRepository _robotComponentsRepository;
        private readonly IStartRobotConstructionService _startRobotConstructionService;


        public SupplyComponentsHandler(ILogger<OrderRobotHandler> logger, IRobotComponentsRepository robotComponentsRepository, IStartRobotConstructionService startRobotConstructionService)
        {
            _logger = logger;
            _robotComponentsRepository = robotComponentsRepository;
            _startRobotConstructionService = startRobotConstructionService;
        }

        public async Task<SupplyComponentsResponse> Handle(SupplyComponentsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding {0} supplied component to the database", request.Components.Length);
            foreach (var component in request.Components)
            {
                await _robotComponentsRepository.CreateRobotComponentAsync(component);
            }

            var constractionQueueMessage = new StartRobotConstructionMessage()
            {
                RobotId = request.Components.First().RobotId,
                RobotConstructingStartTime = DateTime.Now
            };
            await _startRobotConstructionService.AddMessageToQueue(constractionQueueMessage);

            return new SupplyComponentsResponse();
        }
    }
}
