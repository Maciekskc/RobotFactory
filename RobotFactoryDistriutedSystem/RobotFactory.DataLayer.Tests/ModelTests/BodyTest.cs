using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataLayer.Tests.ModelTests
{
    public class BodyTest
    {
        [Fact]
        public void Body_ShouldInitialize_WithComponentTypeBody()
        {
            // Arrange & Act
            var body = new Body();

            // Assert
            Assert.Equal(RobotComponentType.Body, body.ComponentType);
            Assert.Equal(0, body.LegsNumber);
            Assert.Equal(0, body.ArmsNumbers);
        }

        [Fact]
        public void Body_ShouldSerializeAndDeserialize_Correctly()
        {
            // Arrange
            var component = ModelCreators.BodyComponentGenerator.Generate();

            // Act
            var json = JsonConvert.SerializeObject(component);
            var deserializedComponent = JsonConvert.DeserializeObject<Body>(json);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.ArmsNumbers, deserializedComponent.ArmsNumbers);
            Assert.Equal(component.LegsNumber, deserializedComponent.LegsNumber);
            Assert.Equal(RobotComponentType.Body, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }

        [Fact]
        public void Body_ShouldSerializeToBson_Correctly()
        {
            // Arrange
            var component = ModelCreators.BodyComponentGenerator.Generate();

            // Act
            var bson = component.ToBson();
            var deserializedComponent = BsonSerializer.Deserialize<Body>(bson);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.ArmsNumbers, deserializedComponent.ArmsNumbers);
            Assert.Equal(component.LegsNumber, deserializedComponent.LegsNumber);
            Assert.Equal(RobotComponentType.Body, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }
    }
}
