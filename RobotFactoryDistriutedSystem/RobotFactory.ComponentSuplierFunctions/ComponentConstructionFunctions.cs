using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using RobotFactory.DataLayer.Enums;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.QueueObjects;

namespace RobotFactory.ComponentSupplier
{
    public static class ComponentConstructionFunctions
    {
        [FunctionName(nameof(GenerateRobotComponent))]
        public static async Task<RobotComponent> GenerateRobotComponent(
            [ActivityTrigger] RobotComponentOrderItem requestedComponent,
            ILogger log)
        {
            log.LogInformation("Initializing robot component creation. Requested component type: {0}, Params [{1}]",
                requestedComponent.componentType.ToString(),
                string.Join(',',requestedComponent.parameters));
            switch (requestedComponent.componentType)
            {
                case RobotComponentType.Head:
                    await Task.Delay(3000);
                    return await ConstructRobotHead(requestedComponent.parameters);
                    break;

                case RobotComponentType.Body:
                    await Task.Delay(4000);
                    return await ConstructRobotBody(requestedComponent.parameters);
                    break;

                case RobotComponentType.Arm:
                    await Task.Delay(1000);
                    return await ConstructRobotArm(requestedComponent.parameters);
                    break;

                case RobotComponentType.Leg:
                    await Task.Delay(800);
                    return await ConstructRobotLeg(requestedComponent.parameters);
                    break;
                default:
                    throw new InvalidDataException("Unsupported type of component");
            }
            log.LogInformation("Component created");
        }

        private static async Task<RobotComponent> ConstructRobotLeg(string[] requestedComponentParameters)
        {
            await ValidateRobotLegRequest(requestedComponentParameters);
            return new Leg()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                LegSite = requestedComponentParameters[0] switch
                {
                    "Right" => LegSiteType.Right,
                    "Left" => LegSiteType.Left,
                    "Other" => LegSiteType.Other
                }
            };
        }

        private static Task ValidateRobotLegRequest(string[] requestedComponentParameters)
        {

            if (requestedComponentParameters.Length != 1)
                throw new ArgumentException("Robot leg construction request does not contain required parameters");
            var site = requestedComponentParameters[0];

            if (site != "Left" && site != "Right" && site != "Other")
                throw new ArgumentException("Robot leg construction request contain unknown parameters");
            return Task.CompletedTask;
        }

        private static Task<RobotComponent> ConstructRobotArm(string[] requestedComponentParameters)
        {
            ValidateRobotArmRequest(requestedComponentParameters);
            return Task.FromResult<RobotComponent>(new Arm()
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
            });
        }

        private static void ValidateRobotArmRequest(IReadOnlyList<string> requestedComponentParameters)
        {
            if (requestedComponentParameters.Count != 1)
                throw new ArgumentException("Robot arm construction request does not contain required parameters");
            var site = requestedComponentParameters[0];

            if (site != "Left" && site != "Right" && site != "Unified")
                throw new ArgumentException("Robot arm construction request contain unknown parameters");
        }

        private static Task<RobotComponent> ConstructRobotBody(string[] requestedComponentParameters)
        {
            ValidateRobotBodyRequest(requestedComponentParameters);
            return Task.FromResult<RobotComponent>(new Body()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                ArmsNumbers = int.Parse(requestedComponentParameters[0]),
                LegsNumber = int.Parse(requestedComponentParameters[1])
            });
        }

        private static void ValidateRobotBodyRequest(IReadOnlyList<string> requestedComponentParameters)
        {
            if (requestedComponentParameters.Count != 2)
                throw new ArgumentException("Robot body construction request does not contain required parameters");

            if (!int.TryParse(requestedComponentParameters[0], out _) || !int.TryParse(requestedComponentParameters[1], out _))
                throw new ArgumentException("Robot body construction request parameters");
        }

        private static Task<RobotComponent> ConstructRobotHead(string[] requestedComponentParameters)
        {
            ValidateRobotHeadRequest(requestedComponentParameters);
            return Task.FromResult<RobotComponent>(new Head()
            {
                CreatedAt = DateTime.Now,
                Id = null,
                MountedAt = null,
                CPUCoresNumber = Int32.Parse(requestedComponentParameters[0])
            });
        }

        private static void ValidateRobotHeadRequest(string[] requestedComponentParameters)
        {
            if (requestedComponentParameters.Length != 1)
                throw new ArgumentException("Robot head construction request does not contain required parameters");

            if (!int.TryParse(requestedComponentParameters[0], out _))
                throw new ArgumentException("Robot head construction request parameters");
        }
    }
}
