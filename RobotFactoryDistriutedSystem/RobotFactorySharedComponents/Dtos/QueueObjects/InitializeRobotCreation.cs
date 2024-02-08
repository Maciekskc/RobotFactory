namespace RobotFactory.SharedComponents.Dtos.QueueObjects
{
    public class InitializeRobotCreation : BaseRobotCreationMessageModel
    {
        public string Issuer { get; set; }
        public RobotComponentsOrder OrderElements { get; set; }
    }
}
