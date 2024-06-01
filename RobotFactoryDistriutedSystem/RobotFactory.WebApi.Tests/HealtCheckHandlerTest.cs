using Microsoft.Extensions.Logging;
using Moq;
using RobotFactory.WebApi.Handlers.HealthCheck;
using RobotFactorySharedComponents.Dtos.ApiRequests.HealthCheck;

namespace RobotFactory.WebApi.Tests
{
    public class HealtCheckHandlerTest
    {
        private Mock<ILogger<HealthCheckHandler>> _loggerMock;
        private readonly HealthCheckHandler _sut;

        public HealtCheckHandlerTest()
        {
            _loggerMock = new Mock<ILogger<HealthCheckHandler>>();
            _sut = new HealthCheckHandler(_loggerMock.Object);
        }

        private static readonly HealthCheckRequest ValidRequest = new();

        [Fact]
        public void RequestHandler_AnyRequest_ShouldLogRequest()
        {
            _ = _sut.Handle(ValidRequest, CancellationToken.None);

            _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!), Times.Once);
        }
    }
}