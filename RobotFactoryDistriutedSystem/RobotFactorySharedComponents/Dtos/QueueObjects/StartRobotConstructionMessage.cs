namespace RobotFactory.SharedComponents.Dtos.QueueObjects
{
    public class StartRobotConstructionMessage : BaseRobotCreationMessageModel
    {
        public DateTime RobotConstructingStartTime { get; set; }
    }
}
