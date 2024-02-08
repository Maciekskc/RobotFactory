using RobotFactory.DataLayer.Enums;

namespace RobotFactory.DataLayer.Models
{
    public class Head : RobotComponent
    {
        public int CPUCoresNumber { get; set; }

        public Head()
        {
            ComponentType = RobotComponentType.Head;
        }
    }
}
