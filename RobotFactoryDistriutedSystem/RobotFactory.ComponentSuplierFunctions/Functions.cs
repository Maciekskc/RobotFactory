using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using RobotFactory.SharedComponents.Dtos.QueueObjects;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.VisualBasic;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.ComponentSuplierFunctions
{
    public static class Functions
    {
        [FunctionName(nameof(RobotConstructionInitializationConsumer))]
        public static Task RobotConstructionInitializationConsumer(
            [QueueTrigger(@"%StorageQueueName%", Connection = @"StorageQueueConnection")] QueueMessage queueItem,
            [DurableClient] IDurableOrchestrationClient client,
            ILogger log)
        {
            var inputObject = JsonConvert.DeserializeObject<InitializeRobotCreation>(queueItem.MessageText);
            log.LogInformation("Initialize robot creation, processing message {0}", queueItem.MessageId);
            log.LogTrace("Orchestratiing function for message Id: {0}, message content: {1}", queueItem.MessageId, queueItem.MessageText);

            return client.StartNewAsync(nameof(RobotComponentSuplierOrchestrator), inputObject);
        }

        [FunctionName(nameof(RobotComponentSuplierOrchestrator))]
        public static async Task RobotComponentSuplierOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context,
            ILogger log)
        {
            var inputObject = context.GetInput<InitializeRobotCreation>();

            log.LogTrace("Orchestrator receive message: {0}", context.InstanceId);
            await Task.Delay(2000);

            var construnctionTasks = new List<Task<RobotComponent>>();
            // Generate components with different inputs
            foreach (var component in inputObject.OrderElements.Items)
            {
                construnctionTasks.Add(context.CallActivityAsync<RobotComponent>(nameof(GenerateRobotComponent), component));
            }

            var outputs = await Task.WhenAll(construnctionTasks);
            string d = "";
            //// Send the array of outputs to the storage
            //await context.CallActivityAsync("SuplierDeliveryRequest", outputs);

            //// Execute the SubmitMessageToConstructionQueue function
            //await context.CallActivityAsync("SubmitMessageToConstructionQueue", string);
        }

        [FunctionName(nameof(GenerateRobotComponent))]
        public static async Task<RobotComponent> GenerateRobotComponent(
            [OrchestrationTrigger] IDurableOrchestrationContext ctx,
            ILogger log)
        {
            var requestedComponent = ctx.GetInput<RobotComponentOrderItem>();

            RobotComponent robotComponent = null;
            switch (requestedComponent.componentType)
            {
                case RobotComponentType.Head:
                    await Task.Delay(3000);
                    return ConstructRobotHead(requestedComponent.parameters);
                    break;

                case RobotComponentType.Body:
                    await Task.Delay(4000);
                    return ConstructRobotBody(requestedComponent.parameters);
                    break;

                case RobotComponentType.Arm:
                    await Task.Delay(1000);
                    return ConstructRobotArm(requestedComponent.parameters);
                    break;

                case RobotComponentType.Leg:
                    await Task.Delay(800);
                    return ConstructRobotLeg(requestedComponent.parameters);
                    break;
                default:
                    throw new InvalidDataException("Unsuported type of component");
            }
        }

        private static RobotComponent ConstructRobotLeg(string[] requestedComponentParameters)
        {
            ValidateRobotLegRequest(requestedComponentParameters);
            return new Leg()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                ArmSite = requestedComponentParameters[0] switch
                {
                    "Right" => LegSiteType.Right,
                    "Left" => LegSiteType.Left,
                    "Other" => LegSiteType.Other
                }
            };
        }

        private static void ValidateRobotLegRequest(string[] requestedComponentParameters)
        {
            if (requestedComponentParameters.Length != 1)
                throw new ArgumentException("Robot leg construction request does not contain required parameters");
            var site = requestedComponentParameters[0];

            if (site != "Left" || site != "Right" || site != "Other")
                throw new ArgumentException("Robot leg construction request contain unknown parameters");
        }

        private static RobotComponent ConstructRobotArm(string[] requestedComponentParameters)
        {
            ValidateRobotArmRequest(requestedComponentParameters);
            return new Arm()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                ArmSite = requestedComponentParameters[0] switch
                {
                    "Right" => ArmSiteType.Right,
                    "Left" => ArmSiteType.Left,
                    "Unified" => ArmSiteType.Unified
                }
            };
        }

        private static void ValidateRobotArmRequest(string[] requestedComponentParameters)
        {
            if (requestedComponentParameters.Length != 1)
                throw new ArgumentException("Robot arm construction request does not contain required parameters");
            var site = requestedComponentParameters[0];

            if (site != "Left" || site != "Right" || site != "Unified")
                throw new ArgumentException("Robot arm construction request contain unknown parameters");
        }

        private static RobotComponent ConstructRobotBody(string[] requestedComponentParameters)
        {
            ValidateRobotBodyRequest(requestedComponentParameters);
            return new Body()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                ArmsNumbers = Int32.Parse(requestedComponentParameters[0]),
                LegsNumber = Int32.Parse(requestedComponentParameters[1])
            };
        }

        private static void ValidateRobotBodyRequest(string[] requestedComponentParameters)
        {
            if (requestedComponentParameters.Length != 2)
                throw new ArgumentException("Robot body construction request does not contain required parameters");

            if (!Int32.TryParse(requestedComponentParameters[0], out _) || !Int32.TryParse(requestedComponentParameters[1], out _))
                throw new ArgumentException("Robot body construction request parameters");
        }

        private static RobotComponent ConstructRobotHead(string[] requestedComponentParameters)
        {
            ValidateRobotHeadRequest(requestedComponentParameters);
            return new Head()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                CPUCoresNumber = Int32.Parse(requestedComponentParameters[0])
            };
        }

        private static void ValidateRobotHeadRequest(string[] requestedComponentParameters)
        {
            if (requestedComponentParameters.Length != 1)
                throw new ArgumentException("Robot head construction request does not contain required parameters");

            if (!Int32.TryParse(requestedComponentParameters[0], out _))
                throw new ArgumentException("Robot head construction request parameters");
        }
    }
}