using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class Body : RobotComponent
    {
        public int ArmsNumbers { get; set; }
        public int LegsNumber { get; set; }

        public Body()
        {
            ComponentType = RobotComponentType.Body;
        }
    }
}
