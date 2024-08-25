using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataLayer.Tests.ModelTests
{
    public class LegTest
    {
        [Fact]
        public void Leg_ShouldInitialize_WithComponentTypeLeg()
        {
            // Arrange & Act
            var leg = new Leg();

            // Assert
            Assert.Equal(RobotComponentType.Leg, leg.ComponentType);
            Assert.Equal(default(LegSiteType), leg.LegSite);
        }

        [Theory]
        [InlineData(LegSiteType.Left)]
        [InlineData(LegSiteType.Right)]
        public void Leg_ShouldHandleEnumValues_Correctly(LegSiteType LegSite)
        {
            // Arrange & Act
            var leg = new Leg
            {
                LegSite = LegSite
            };

            // Assert
            Assert.Equal(LegSite, leg.LegSite);
        }

        [Fact]
        public void Leg_ShouldSerializeAndDeserialize_Correctly()
        {
            // Arrange
            var component = ModelCreators.LegComponentGenerator.Generate();

            // Act
            var json = JsonConvert.SerializeObject(component);
            var deserializedComponent = JsonConvert.DeserializeObject<Leg>(json);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.LegSite, deserializedComponent.LegSite);
            Assert.Equal(RobotComponentType.Leg, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }

        [Fact]
        public void Leg_ShouldSerializeToBson_Correctly()
        {
            // Arrange
            var component = ModelCreators.LegComponentGenerator.Generate();

            // Act
            var bson = component.ToBson();
            var deserializedComponent = BsonSerializer.Deserialize<Leg>(bson);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.LegSite, deserializedComponent.LegSite);
            Assert.Equal(RobotComponentType.Leg, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }
    }
}
