using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class Robot
    {
        public Guid RobotId  { get; set; }
        public RobotConstrucionStatus ConstructionStatus { get; set; }
        public Head Head { get; set; }
        public Body Body { get; set; }
        public ICollection<Arm> Arms { get; set; }
        public ICollection<Leg> Legs { get; set; }  
    }
}
