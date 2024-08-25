using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataLayer.Tests.ModelTests
{
    public class RobotComponentTest
    {
        [Fact]
        public void RobotComponent_ShouldInitialize_WithDefaultValues()
        {
            // Arrange & Act
            var component = new RobotComponent();

            // Assert
            Assert.Null(component.Id);
            Assert.Null(component.RobotId);
            Assert.Equal(default(RobotComponentType), component.ComponentType);
            Assert.Equal(default(DateTime), component.CreatedAt);
            Assert.Null(component.MountedAt);
        }

        [Fact]
        public void RobotComponent_ShouldSerializeAndDeserialize_Correctly()
        {
            // Arrange
            var component = ModelCreators.RobotComponentGenerator.Generate();

            // Act
            var json = JsonConvert.SerializeObject(component);
            var deserializedComponent = JsonConvert.DeserializeObject<RobotComponent>(json);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.ComponentType, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }

        [Fact]
        public void RobotComponent_ShouldSerializeToBson_Correctly()
        {
            // Arrange
            var component = ModelCreators.RobotComponentGenerator.Generate();

            // Act
            var bson = component.ToBson();
            var deserializedComponent = BsonSerializer.Deserialize<RobotComponent>(bson);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.ComponentType, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }
    }
}