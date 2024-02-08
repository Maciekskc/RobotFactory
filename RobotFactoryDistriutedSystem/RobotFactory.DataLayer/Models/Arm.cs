using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class Arm : RobotComponent
    {
        public ArmSiteType ArmSite { get; set; }

        public Arm()
        {
            ComponentType = RobotComponentType.Arm;
        }
    }
}
