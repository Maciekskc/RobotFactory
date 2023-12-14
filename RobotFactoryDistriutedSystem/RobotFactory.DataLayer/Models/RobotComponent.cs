using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class RobotComponent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [JsonPropertyName("Value")]
        protected RobotComponentType ComponentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? MountedAt { get; set; }
    }
}
