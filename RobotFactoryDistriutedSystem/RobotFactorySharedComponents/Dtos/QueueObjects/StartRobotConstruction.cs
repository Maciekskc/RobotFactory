namespace RobotFactory.SharedComponents.Dtos.QueueObjects
{
    public class StartRobotConstruction : BaseRobotCreationMessageModel
    {
        public DateTime RobotConstructingStartTime { get; set; }
    }
}
