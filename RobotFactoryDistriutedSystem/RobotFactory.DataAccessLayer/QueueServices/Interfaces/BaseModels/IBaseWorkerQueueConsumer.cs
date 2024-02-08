using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels
{
    public interface IBaseWorkerQueueConsumer<T> where T : class
    {
        public Task<List<QueueMessageWrapper<T>>> ReadMessagesAsync(int maxMessageCount);

        public Task RemoveMessagesFromQueueAsync(QueueMessageWrapper<T> message);
    }
}
