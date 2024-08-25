using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataLayer.Tests.ModelTests
{
    public class RobotTest
    {
        [Fact]
        public void Robot_ShouldInitialize_WithDefaultValues()
        {
            // Arrange & Act
            var robot = new Robot();

            // Assert
            Assert.Equal(default(RobotConstrucionStatus), robot.ConstructionStatus);
            Assert.Null(robot.Head);
            Assert.Null(robot.Body);
            Assert.NotNull(robot.Arms);
            Assert.Empty(robot.Arms);
            Assert.NotNull(robot.Legs);
            Assert.Empty(robot.Legs);
            Assert.Equal(default(DateTime), robot.OrderedAt);
            Assert.Null(robot.FinalizedAt);
        }

        [Fact]
        public void Robot_ShouldSerializeAndDeserialize_Correctly()
        {
            // Arrange
            var robot = ModelCreators.RobotGenerator.Generate();

            // Act
            var bson = robot.ToBson();
            var deserializedRobot = BsonSerializer.Deserialize<Robot>(bson);

            // Assert
            Assert.Equal(robot.Id, deserializedRobot.Id);
            Assert.Equal(robot.ConstructionStatus, deserializedRobot.ConstructionStatus);
            Assert.Equal(robot.Head?.ComponentType, deserializedRobot.Head?.ComponentType);
            Assert.Equal(robot.Body?.ComponentType, deserializedRobot.Body?.ComponentType);
            Assert.Equal(robot.Arms.Count, deserializedRobot.Arms.Count);
            Assert.Equal(robot.Legs.Count, deserializedRobot.Legs.Count);
            Assert.Equal(robot.OrderedAt, deserializedRobot.OrderedAt, TimeSpan.FromMilliseconds(1));
            if(robot.ConstructionStatus == RobotConstrucionStatus.Constructed)
                Assert.Equal(robot.FinalizedAt!.Value, deserializedRobot.FinalizedAt!.Value, TimeSpan.FromMilliseconds(1));
        }

        [Theory]
        [InlineData(RobotConstrucionStatus.Initialized)]
        [InlineData(RobotConstrucionStatus.ConstructionStarted)]
        [InlineData(RobotConstrucionStatus.AwaitingComponents)]
        [InlineData(RobotConstrucionStatus.Constructed)]

        public void Robot_ShouldHandleConstructionStatus_Correctly(RobotConstrucionStatus status)
        {
            // Arrange
            var robot = ModelCreators.RobotGenerator.Generate();
            robot.ConstructionStatus = status;

            // Act
            var bson = robot.ToBson();
            var deserializedRobot = BsonSerializer.Deserialize<Robot>(bson);

            // Assert
            Assert.Equal(status, deserializedRobot.ConstructionStatus);
        }
    }
}
