using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class RobotComponent
    {
        public Guid RobotComponentId { get; set; }
        protected RobotComponentType ComponentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime MountedAt { get; set; }

    }
}
