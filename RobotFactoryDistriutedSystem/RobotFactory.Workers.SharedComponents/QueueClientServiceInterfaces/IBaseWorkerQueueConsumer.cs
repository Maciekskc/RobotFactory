namespace RobotFactory.Workers.SharedComponents.QueueClientServiceInterfaces
{
    public interface IBaseWorkerQueueConsumer<T> where T : class
    {
        public Task<List<T>> ReadMessagesAsync(int maxMessageCount);
    }
}
