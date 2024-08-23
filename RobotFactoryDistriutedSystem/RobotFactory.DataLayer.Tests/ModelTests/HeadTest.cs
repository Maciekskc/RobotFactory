using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataLayer.Tests.ModelTests
{
    public class HeadTest
    {
        [Fact]
        public void Head_ShouldInitialize_WithComponentTypeHead()
        {
            // Arrange & Act
            var head = new Head();

            // Assert
            Assert.Equal(RobotComponentType.Head, head.ComponentType);
            Assert.Equal(0, head.CPUCoresNumber);
        }

        [Fact]
        public void Head_ShouldSerializeAndDeserialize_Correctly()
        {
            // Arrange
            var component = ModelCreators.HeadComponentGenerator.Generate();

            // Act
            var json = JsonConvert.SerializeObject(component);
            var deserializedComponent = JsonConvert.DeserializeObject<Head>(json);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.CPUCoresNumber, deserializedComponent.CPUCoresNumber);
            Assert.Equal(RobotComponentType.Head, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }

        [Fact]
        public void Head_ShouldSerializeToBson_Correctly()
        {
            // Arrange
            var component = ModelCreators.HeadComponentGenerator.Generate();

            // Act
            var bson = component.ToBson();
            var deserializedComponent = BsonSerializer.Deserialize<Head>(bson);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.CPUCoresNumber, deserializedComponent.CPUCoresNumber);
            Assert.Equal(RobotComponentType.Head, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }
    }
}
