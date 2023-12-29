using Azure.Storage.Queues;

namespace RobotFactory.Workers.SharedComponents.QueueClientInterfaces
{
    public interface IBaseWorkerQueuePublisher
    {
        public Task PublishMessageAsync(string message);
    }
}
