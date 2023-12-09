using MediatR;

namespace RobotFactorySharedComponents.Dtos.ApiRequests.HealthCheck
{
    public record HealthCheckRequest() : IRequest<HealthCheckResponse>;
}
