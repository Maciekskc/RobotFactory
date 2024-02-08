namespace RobotFactory.SharedComponents.Dtos.QueueObjects
{
    public class QueueMessageWrapper<T> where T : class
    {
        public string MessageId { get; set; }
        public string MessagePopReceipt { get; set; }
        public T MessageObject { get; set; }
    }
}
