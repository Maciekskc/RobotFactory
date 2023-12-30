namespace RobotFactory.DataAccessLayer.QueueServices.Interfaces.BaseModels
{
    public interface IBaseWorkerQueuePublisher
    {
        public Task PublishMessageAsync(string message);
    }
}
