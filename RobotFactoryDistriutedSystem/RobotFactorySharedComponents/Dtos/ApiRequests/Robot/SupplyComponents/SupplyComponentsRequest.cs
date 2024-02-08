using RobotFactory.DataLayer.Models;
using MediatR;

namespace RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.SupplyComponents
{
    public class SupplyComponentsRequest : IRequest<SupplyComponentsResponse>
    {
        public RobotComponent[] Components { get; set; }
    }
}
