using RobotFactory.DataLayer.Enums;

namespace RobotFactory.SharedComponents.Dtos.QueueObjects
{
    public class RobotComponentsOrder
    {
        public RobotComponentOrderItem[] Items { get; set; }
    }

    public record RobotComponentOrderItem(RobotComponentType componentType, string[] parameters);
}
