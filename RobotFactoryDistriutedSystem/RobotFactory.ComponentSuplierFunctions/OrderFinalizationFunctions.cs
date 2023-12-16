using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace RobotFactory.ComponentSupplier
{
    public static class OrderFinalizationFunctions
    {
        [FunctionName(nameof(SendMessageToStartConstructionQueue))]
        public static async Task SendMessageToStartConstructionQueue(
            [ActivityTrigger] string robotId,
            ILogger log)
        {
            log.LogInformation("Attempting to send request to robot factory queue to start processing robot: RobotId {0}", robotId);
        }
    }
}
