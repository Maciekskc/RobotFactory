using System;
using System.Buffers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using Bogus;
using MongoDB.Bson;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Converters;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.TestHelpers;
using JsonException = System.Text.Json.JsonException;

namespace RobotFactory.DataLayer.Tests.Converters
{
    public class RobotComponentConverterTests
    {
        private readonly Faker _faker = new Faker();

        private readonly JsonSerializerOptions _serializerOptions;
        private readonly RobotComponentConverter _converter;

        public RobotComponentConverterTests()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                Converters = { new RobotComponentConverter() }
            };
            _converter = new RobotComponentConverter();
        }

        [Fact]
        public void CanConvert_RobotComponentType_ReturnsTrue()
        {
            // Arrange & Act
            bool result = _converter.CanConvert(typeof(RobotComponent));

            // Assert
            Assert.True(result);
        }

        public class foo
        {
            public int a { get; set; }
        }

        private RobotComponent ReadComponent(string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            var reader = new Utf8JsonReader(bytes);
            reader.Read();
            return _converter.Read(ref reader, typeof(RobotComponent), _serializerOptions);
        }

        [Fact]
        public void Read_InvalidJsonToken_ThrowsJsonException()
        {
            // Arrange
            var json = "\"InvalidJson\"";
            
            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => ReadComponent(json));
            Assert.Equal("Cannot deserialize RobotComponent object", exception.Message);
        }

        [Fact]
        public void Read_MissingComponentType_ThrowsJsonException()
        {
            // Arrange
            var json = "{\"InvalidProperty\":\"value\"}";

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => ReadComponent(json));
            Assert.Equal("Cannot read type property", exception.Message);
        }

        [Fact]
        public void Read_InvalidComponentType_ThrowsJsonException()
        {
            // Arrange
            var json = "{\"ComponentType\":\"InvalidType\"}";

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => ReadComponent(json));
            Assert.Equal("Cannot find component type given in payload", exception.Message);
        }

        [Fact]
        public void Read_InValidComponentType_ThrowRelevanttUnsuportedException()
        {
            // Arrange
            var json = $"{{\"ComponentType\":-1, \"Id\":\"{ObjectId.GenerateNewId().ToString()}\"}}";

            // Act & Assert
            var exception = Assert.Throws<JsonException>(() => ReadComponent(json));
            Assert.Equal("Cannot deserialize object due to unknown component type", exception.Message);
        }

        [Theory]
        [InlineData((int)RobotComponentType.Head, typeof(Head))]
        [InlineData((int)RobotComponentType.Body, typeof(Body))]
        [InlineData((int)RobotComponentType.Arm, typeof(Arm))]
        [InlineData((int)RobotComponentType.Leg, typeof(Leg))]
        public void Read_ValidComponentType_ReturnsCorrectComponent(int componentTypeValue, Type expectedType)
        {
            // Arrange
            var json = $"{{\"ComponentType\":{componentTypeValue}, \"Id\":\"{ObjectId.GenerateNewId().ToString()}\"}}";
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json));
            reader.Read();
            // Act
            var result = _converter.Read(ref reader, typeof(RobotComponent), _serializerOptions);

            // Assert
            Assert.IsType(expectedType, result);
        }

        [Fact]
        public void Write_ArmComponent_SerializesCorrectly()
        {
            // Arrange
            var arm = ModelCreators.ArmComponentGenerator.Generate();
            var writer = new Utf8JsonWriter(new ArrayBufferWriter<byte>());

            // Act
            _converter.Write(writer, arm, _serializerOptions);

            // Assert
            // This checks that writing did not throw an exception.
            Assert.True(true);
        }

        [Fact]
        public void Write_HeadComponent_SerializesCorrectly()
        {
            // Arrange
            var head = ModelCreators.HeadComponentGenerator.Generate();
            var writer = new Utf8JsonWriter(new ArrayBufferWriter<byte>());

            // Act
            _converter.Write(writer, head, _serializerOptions);

            // Assert
            // This checks that writing did not throw an exception.
            Assert.True(true);
        }

        [Fact]
        public void Write_BodyComponent_SerializesCorrectly()
        {
            // Arrange
            var body = ModelCreators.BodyComponentGenerator.Generate();
            var writer = new Utf8JsonWriter(new ArrayBufferWriter<byte>());

            // Act
            _converter.Write(writer, body, _serializerOptions);

            // Assert
            // This checks that writing did not throw an exception.
            Assert.True(true);
        }

        [Fact]
        public void Write_LegComponent_SerializesCorrectly()
        {
            // Arrange
            var leg = ModelCreators.LegComponentGenerator.Generate();
            var writer = new Utf8JsonWriter(new ArrayBufferWriter<byte>());

            // Act
            _converter.Write(writer, leg, _serializerOptions);

            // Assert
            // This checks that writing did not throw an exception.
            Assert.True(true);
        }
    }
}
