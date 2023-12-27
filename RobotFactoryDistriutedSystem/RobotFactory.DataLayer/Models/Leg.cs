using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class Leg : RobotComponent
    {
        public LegSiteType LegSite { get; set; }

        public Leg()
        {
            ComponentType = RobotComponentType.Leg;
        }
    }
}
