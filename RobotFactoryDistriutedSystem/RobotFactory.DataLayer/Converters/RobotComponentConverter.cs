using System.Text.Json;
using System.Text.Json.Serialization;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.DataLayer.Converters
{
    internal class RobotComponentConverter : JsonConverter<RobotComponent>
    {
        public override bool CanConvert(Type typeToConvert) => typeof(RobotComponent).IsAssignableFrom(typeToConvert);
        public override RobotComponent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException("Cannot deserialize RobotComponent object");
            }
            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                if (!jsonDocument.RootElement.TryGetProperty(nameof(RobotComponent.ComponentType), out var typeProperty))
                {
                    throw new JsonException("Cannot read type property");
                }
                var jsonRobotComponent = jsonDocument.RootElement.GetRawText();

                if (!Enum.TryParse(typeProperty.ToString(), out RobotComponentType type))
                    throw new JsonException("Cannot find component type given in payload");

                return type switch
                {
                    RobotComponentType.Head => (Head)JsonSerializer.Deserialize(jsonRobotComponent, typeof(Head)),
                    RobotComponentType.Body => (Body)JsonSerializer.Deserialize(jsonRobotComponent, typeof(Body)),
                    RobotComponentType.Arm => (Arm)JsonSerializer.Deserialize(jsonRobotComponent, typeof(Arm)),
                    RobotComponentType.Leg => (Leg)JsonSerializer.Deserialize(jsonRobotComponent, typeof(Leg)),
                    _ => (RobotComponent)JsonSerializer.Deserialize(jsonRobotComponent, typeof(RobotComponent))
                };
            }
        }

        public override void Write(
            Utf8JsonWriter writer, RobotComponent robotComponent, JsonSerializerOptions options)
        {
            if (robotComponent is Arm arm)
            {
                JsonSerializer.Serialize(writer, arm);
            }
            else if (robotComponent is Leg leg)
            {
                JsonSerializer.Serialize(writer, leg);
            }
            else if (robotComponent is Head head)
            {
                JsonSerializer.Serialize(writer, head);
            }
            else if (robotComponent is Body body)
            {
                JsonSerializer.Serialize(writer, body);
            }
        }
    }
}
