using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.ComponentSupplier
{
    public static class OrchestrationFunctions
    {

        [FunctionName(nameof(RobotConstructionInitializationConsumer))]
        public static async Task RobotConstructionInitializationConsumer(
            [QueueTrigger(@"%StorageQueueName%", Connection = @"StorageQueueConnection")] QueueMessage queueItem,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            var inputObject = JsonConvert.DeserializeObject<InitializeRobotCreation>(queueItem.MessageText);
            log.LogInformation("Initialize robot creation, processing message {0}", queueItem.MessageId);
            log.LogTrace("Orchestrating function for message Id: {0}, message content: {1}", queueItem.MessageId, queueItem.MessageText);

            await client.StartNewAsync(nameof(RobotComponentsSupplierOrchestrator), inputObject);
        }

        [FunctionName(nameof(RobotComponentsSupplierOrchestrator))]
        public static async Task RobotComponentsSupplierOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var inputObject = context.GetInput<InitializeRobotCreation>();

            log.LogTrace("Orchestrator receive message: {0}", context.InstanceId);

            var constructionTasks = new List<Task<RobotComponent>>();
            foreach (var component in inputObject.OrderElements.Items)
            {
                constructionTasks.Add(context.CallActivityAsync<RobotComponent>(nameof(ComponentConstructionFunctions.GenerateRobotComponent), component));
            }

            var outputs = await Task.WhenAll(constructionTasks);
            Array.ForEach(outputs, c => c.RobotId = inputObject.RobotId);

            //// Send the array of outputs to the storage
            await context.CallActivityAsync(nameof(ComponentSupplierFunctions.SendComponentsToTheDatabaseStore), outputs);

            // Execute the SubmitMessageToConstructionQueue function
            await context.CallActivityAsync(nameof(OrderFinalizationFunctions.SendMessageToStartConstructionQueue), inputObject.RobotId);
        }
    }
}