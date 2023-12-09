using MediatR;
using RobotFactorySharedComponents;
using RobotFactorySharedComponents.Dtos.ApiRequests.HealthCheck;

namespace RobotFactoryWebApi.Handlers
{
    public class HealthCheckHandler : IRequestHandler<HealthCheckRequest, HealthCheckResponse>
    {
        private readonly ILogger<HealthCheckHandler> _logger;

        public HealthCheckHandler(ILogger<HealthCheckHandler> logger)
        {
            _logger = logger;
        }

        public Task<HealthCheckResponse> Handle(HealthCheckRequest request, CancellationToken cancellationToken)
        {
            var status = HealthCheckStatusEnum.Healthy;
            _logger.LogInformation("Application returned health status: {0}", status.ToString());
            return Task.FromResult(new HealthCheckResponse(status));
        }
    }
}
