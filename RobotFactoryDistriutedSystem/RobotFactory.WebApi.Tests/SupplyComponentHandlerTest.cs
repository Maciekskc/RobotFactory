using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.OrderRobots;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.SupplyComponents;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.WebApi.Handlers.Robot;

namespace RobotFactory.WebApi.Tests
{
    public class SupplyComponentsHandlerTest
    {
        private Mock<ILogger<SupplyComponentsHandler>> _loggerMock;
        private Mock<IRobotComponentsRepository> _repository;
        private Mock<IStartRobotConstructionService> _queueServiceMock;

        private readonly SupplyComponentsHandler _sut;

        private static readonly Faker _faker = new Faker();
        private static string robotId = _faker.Random.String(16);

        public SupplyComponentsHandlerTest()
        {
            _loggerMock = new Mock<ILogger<SupplyComponentsHandler>>();
            _repository = new Mock<IRobotComponentsRepository>(MockBehavior.Loose);
            _queueServiceMock = new Mock<IStartRobotConstructionService>(MockBehavior.Loose);
            _sut = new SupplyComponentsHandler(_loggerMock.Object, _repository.Object, _queueServiceMock.Object);
        }


        private static readonly Body ValidBody = new ()
        {
            ArmsNumbers = 2,
            LegsNumber = 2,
            Id = _faker.Random.String(16),
            ComponentType = RobotComponentType.Body,
            CreatedAt = _faker.Date.Past(1),
            MountedAt = null,
            RobotId = robotId
        };

        private static readonly Head ValidHead = new()
        {
            ComponentType = RobotComponentType.Head,
            CPUCoresNumber = 2^_faker.Random.Number(4),
            Id = _faker.Random.String(16),
            CreatedAt = _faker.Date.Past(1),
            MountedAt = null,
            RobotId = robotId
        };

        private static readonly Leg ValidRightLeg = new()
        {
            ComponentType = RobotComponentType.Leg,
            LegSite = LegSiteType.Right,
            Id = _faker.Random.String(16),
            CreatedAt = _faker.Date.Past(1),
            MountedAt = null,
            RobotId = robotId
        };

        private static readonly Leg ValidLeftLeg = new()
        {
            ComponentType = RobotComponentType.Leg,
            LegSite = LegSiteType.Left,
            Id = _faker.Random.String(16),
            CreatedAt = _faker.Date.Past(1),
            MountedAt = null,
            RobotId = robotId
        };

        private static readonly Arm ValidRightArm = new()
        {
            ComponentType = RobotComponentType.Arm,
            ArmSite = ArmSiteType.Right,
            Id = _faker.Random.String(16),
            CreatedAt = _faker.Date.Past(1),
            MountedAt = null,
            RobotId = robotId
        };

        private static readonly Arm ValidLeftArm = new()
        {
            ComponentType = RobotComponentType.Arm,
            ArmSite = ArmSiteType.Left,
            Id = _faker.Random.String(16),
            CreatedAt = _faker.Date.Past(1),
            MountedAt = null,
            RobotId = robotId
        };

        private static readonly SupplyComponentsRequest ValidRequest = new()
        {
            Components = new RobotComponent[]
            {
                ValidBody,
                ValidHead,
                ValidLeftArm,
                ValidRightArm,
                ValidLeftLeg,
                ValidRightLeg
            }
        };

        [Fact]
        public async Task Handle_AnyRequest_ShouldLogInformation()
        {
            _ = await _sut.Handle(ValidRequest, CancellationToken.None);

            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!), Times.Once);
        }

        [Fact]
        public async Task Handle_AnyRequest_DatabaseIntegrationTriggeredProperly()
        {
            var handlerResponse = await _sut.Handle(ValidRequest, CancellationToken.None);

            _repository.Verify(x => x.CreateRobotComponentAsync(It.Is<Body>(rc =>
                rc.ComponentType == RobotComponentType.Body
            )));

            _repository.Verify(x => x.CreateRobotComponentAsync(It.Is<Head>(rc =>
                rc.ComponentType == RobotComponentType.Head
            )));

            _repository.Verify(x => x.CreateRobotComponentAsync(It.Is<Arm>(rc =>
                rc.ComponentType == RobotComponentType.Arm &&
                rc.ArmSite == ArmSiteType.Right
            )));

            _repository.Verify(x => x.CreateRobotComponentAsync(It.Is<Arm>(rc =>
                rc.ComponentType == RobotComponentType.Arm &&
                rc.ArmSite == ArmSiteType.Left
            )));

            _repository.Verify(x => x.CreateRobotComponentAsync(It.Is<Leg>(rc =>
                rc.ComponentType == RobotComponentType.Leg &&
                rc.LegSite == LegSiteType.Right
            )));

            _repository.Verify(x => x.CreateRobotComponentAsync(It.Is<Leg>(rc =>
                rc.ComponentType == RobotComponentType.Leg &&
                rc.LegSite == LegSiteType.Left
            )));
        }

        [Fact]
        public void Handle_AnyRequest_QueueIntegrationTriggeredProperly()
        {
            _ = _sut.Handle(ValidRequest, CancellationToken.None);

            _queueServiceMock.Verify(x => x.AddMessageToQueue(It.Is<StartRobotConstructionMessage>(r =>
                r.RobotId == robotId &&
                r.RobotConstructingStartTime != default(DateTime)
            )));
        }
    }
}
