namespace RobotFactory.SharedComponents.Dtos.QueueObjects
{
    public class FinalizeRobotConstructionMessage : BaseRobotCreationMessageModel
    {
        public DateTime RobotConstructingStartTime { get; set; }
    }
}
