namespace RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels
{
    public interface IBaseWorkerQueueConsumer<T> where T : class
    {
        public Task<List<T>> ReadMessagesAsync(int maxMessageCount);
    }
}
