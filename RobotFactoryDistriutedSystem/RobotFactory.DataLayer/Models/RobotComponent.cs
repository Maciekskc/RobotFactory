using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RobotFactory.DataLayer.Converters;
using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    [JsonConverter(typeof(RobotComponentConverter))]
    public abstract class RobotComponent
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? RobotId { get; set; }
        public RobotComponentType ComponentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? MountedAt { get; set; }
    }
}
