using Bogus;
using Microsoft.Extensions.Logging;
using Moq;
using RobotFactory.DataAccessLayer.QueueServices.Interfaces;
using RobotFactory.DataAccessLayer.Repositories.Interfaces;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.OrderRobots;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using RobotFactory.WebApi.Handlers.Robot;

namespace RobotFactory.WebApi.Tests
{
    public class OrderRobotHandlerTest
    {
        private Mock<ILogger<OrderRobotHandler>> _loggerMock;
        private Mock<IRobotRepository> _repository;
        private Mock<IInitializeRobotCreationQueueService> _queueServiceMock;
        private readonly OrderRobotHandler _sut;

        private readonly string robotId;

        public OrderRobotHandlerTest()
        {

            robotId = new Faker().Random.String(16);
            _loggerMock = new Mock<ILogger<OrderRobotHandler>>();
            _repository = new Mock<IRobotRepository>();
            _repository
                .Setup(x=>x.CreateRobotAsync(It.IsAny<Robot>()))
                .Callback<Robot>(r => r.Id = robotId)
                .Returns(Task.CompletedTask);
            _queueServiceMock = new Mock<IInitializeRobotCreationQueueService>(MockBehavior.Loose);
            _sut = new OrderRobotHandler(_loggerMock.Object, _repository.Object, _queueServiceMock.Object);
        }

        private static readonly OrderRobotRequest ValidRequest = new();

        [Fact]
        public async Task Handle_AnyRequest_ShouldLogInformation()
        {
            _ = await _sut.Handle(ValidRequest, CancellationToken.None);

            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_AnyRequest_DatabaseIntegrationTriggeredProperly()
        {
            var handlerResponse = await _sut.Handle(ValidRequest, CancellationToken.None);

            _repository.Verify(x => x.CreateRobotAsync(It.Is<Robot>(r =>
                r.ConstructionStatus == RobotConstrucionStatus.AwaitingComponents &&
                r.OrderedAt != default(DateTime)
            )));

            Assert.Equal(robotId, handlerResponse.Id);
        }

        [Fact]
        public void Handle_AnyRequest_QueueIntegrationTriggeredProperly()
        {
            _ = _sut.Handle(ValidRequest, CancellationToken.None);

            _queueServiceMock.Verify(x => x.AddMessageToQueue(It.Is<InitializeRobotCreation>(r =>
                r.RobotId == robotId &&
                r.Issuer == "HardcodedInWebApiApplication" &&
                r.OrderElements.Items != Array.Empty<RobotComponentOrderItem>() &&
                r.OrderElements.Items.Any(i=>i.ComponentType == RobotComponentType.Body) &&
                r.OrderElements.Items.Any(i=>i.ComponentType == RobotComponentType.Head) &&
                r.OrderElements.Items.Any(i=>i.ComponentType == RobotComponentType.Arm) &&
                r.OrderElements.Items.Any(i=>i.ComponentType == RobotComponentType.Leg) 
            )));
        }
    }
}
