using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using RobotFactory.DataAccessLayer.Repositories;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataAccessLayer.Tests.Repositories;

public class RobotRepositoryTest
{
    private readonly Mock<IMongoCollection<Robot>> _mockRobotCollection;
    private RobotRepository _robotRepository;

    public RobotRepositoryTest()
    {
        _mockRobotCollection = new Mock<IMongoCollection<Robot>>().SetupAllProperties();

        _robotRepository = new RobotRepository(_mockRobotCollection.Object);
    }

    [Fact]
    public async Task CreateRobotAsync_ShouldInsertRobot()
    {
        // Arrange
        var newRobot = ModelCreators.RobotGenerator.Generate();

        // Act
        await _robotRepository.CreateRobotAsync(newRobot);

        // Assert
        _mockRobotCollection.Verify(collection => collection.InsertOneAsync(newRobot, null, default), Times.Once);
    }

    [Theory]
    [InlineData(RobotComponentType.Head)]
    [InlineData(RobotComponentType.Body)]
    [InlineData(RobotComponentType.Arm)]
    [InlineData(RobotComponentType.Leg)]
    public async Task AddRobotComponentAsync_HeadComponent_ShouldAddComponentToRobot(RobotComponentType componentType)
    {
        // Arrange
        var robotId = ObjectId.GenerateNewId().ToString();
        RobotComponent newRobotComponent = componentType switch
        {
            RobotComponentType.Head => ModelCreators.HeadComponentGenerator.Generate(),
            RobotComponentType.Body => ModelCreators.BodyComponentGenerator.Generate(),
            RobotComponentType.Arm => ModelCreators.ArmComponentGenerator.Generate(),
            RobotComponentType.Leg => ModelCreators.LegComponentGenerator.Generate(),
            _ => throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null)
        };

        Type type = componentType switch
        {
            RobotComponentType.Head => typeof(Head),
            RobotComponentType.Body => typeof(Head),
            RobotComponentType.Arm => typeof(Head),
            RobotComponentType.Leg => typeof(Head),
            _ => throw new ArgumentOutOfRangeException(nameof(componentType), componentType, null)
        };

        // Act
        await _robotRepository.AddRobotComponentAsync(robotId!, type, newRobotComponent);

        // Assert
        _mockRobotCollection.Verify(
            collection => collection.UpdateOneAsync(It.IsAny<FilterDefinition<Robot>>(),
                It.IsAny<UpdateDefinition<Robot>>(), null, default), Times.Once);
    }

    [Fact]
    public async Task UpdateRobotProperty_ShouldUpdateProperty()
    {
        // Arrange
        var robotId = ObjectId.GenerateNewId().ToString();

        // Act
        await _robotRepository.UpdateRobotProperty(robotId, x => x.ConstructionStatus,
            RobotConstrucionStatus.Constructed);

        // Assert
        _mockRobotCollection.Verify(
            collection => collection.UpdateOneAsync(It.IsAny<FilterDefinition<Robot>>(),
                It.IsAny<UpdateDefinition<Robot>>(), null, default), Times.Once);
    }
}