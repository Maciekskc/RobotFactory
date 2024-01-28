using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class Robot
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public RobotConstrucionStatus ConstructionStatus { get; set; }
        public Head Head { get; set; }
        public Body Body { get; set; }
        public ICollection<Arm> Arms { get; set; } = new List<Arm>();
        public ICollection<Leg> Legs { get; set; } = new List<Leg>();

        public DateTime OrderedAt { get; set; }
        public DateTime? FinalizedAt { get; set; }
    }
}
