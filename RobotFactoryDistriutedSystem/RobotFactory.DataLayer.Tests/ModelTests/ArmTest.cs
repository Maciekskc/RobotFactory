using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;

namespace RobotFactory.DataLayer.Tests.ModelTests
{
    public class ArmTest
    {
        [Fact]
        public void Arm_ShouldInitialize_WithComponentTypeArm()
        {
            // Arrange & Act
            var arm = new Arm();

            // Assert
            Assert.Equal(RobotComponentType.Arm, arm.ComponentType);
            Assert.Equal(default(ArmSiteType), arm.ArmSite);
        }

        [Theory]
        [InlineData(ArmSiteType.Left)]
        [InlineData(ArmSiteType.Right)]
        public void Arm_ShouldHandleEnumValues_Correctly(ArmSiteType armSite)
        {
            // Arrange & Act
            var arm = new Arm
            {
                ArmSite = armSite
            };

            // Assert
            Assert.Equal(armSite, arm.ArmSite);
        }

        [Fact]
        public void Arm_ShouldSerializeAndDeserialize_Correctly()
        {
            // Arrange
            var component = ModelCreators.ArmComponentGenerator.Generate();

            // Act
            var json = JsonConvert.SerializeObject(component);
            var deserializedComponent = JsonConvert.DeserializeObject<Arm>(json);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.ArmSite, deserializedComponent.ArmSite);
            Assert.Equal(RobotComponentType.Arm, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }

        [Fact]
        public void Arm_ShouldSerializeToBson_Correctly()
        {
            // Arrange
            var component = ModelCreators.ArmComponentGenerator.Generate();

            // Act
            var bson = component.ToBson();
            var deserializedComponent = BsonSerializer.Deserialize<Arm>(bson);

            // Assert
            Assert.Equal(component.Id, deserializedComponent.Id);
            Assert.Equal(component.RobotId, deserializedComponent.RobotId);
            Assert.Equal(component.ArmSite, deserializedComponent.ArmSite);
            Assert.Equal(RobotComponentType.Arm, deserializedComponent.ComponentType);
            Assert.Equal(component.CreatedAt, deserializedComponent.CreatedAt, TimeSpan.FromMilliseconds(1));
            Assert.Equal(component.MountedAt, deserializedComponent.MountedAt);
        }
    }
}
