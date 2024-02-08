namespace RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels
{
    public interface IBaseWorkerQueuePublisher<T>
    {
        public Task PublishMessageAsync(string message);
    }
}
