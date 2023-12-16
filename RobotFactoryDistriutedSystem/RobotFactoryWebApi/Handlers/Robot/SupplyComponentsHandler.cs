using MediatR;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.SupplyComponents;

namespace RobotFactory.WebApi.Handlers.Robot
{
    public class SupplyComponentsHandler : IRequestHandler<SupplyComponentsRequest, SupplyComponentsResponse>
    {
        private readonly ILogger<OrderRobotHandler> _logger;
        private readonly IRobotComponentsRepository _robotComponentsRepository;

        public SupplyComponentsHandler(ILogger<OrderRobotHandler> logger, IRobotComponentsRepository robotComponentsRepository)
        {
            _logger = logger;
            _robotComponentsRepository = robotComponentsRepository;
        }

        public async Task<SupplyComponentsResponse> Handle(SupplyComponentsRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding {0} supplied component to the database", request.Components.Length);
            foreach (var component in request.Components)
            {
                await _robotComponentsRepository.CreateRobotComponentAsync(component);
            }
            return new SupplyComponentsResponse();
        }
    }
}
