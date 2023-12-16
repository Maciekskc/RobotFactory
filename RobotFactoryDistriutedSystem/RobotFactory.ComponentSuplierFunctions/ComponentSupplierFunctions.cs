using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RobotFactory.DataLayer.Models;
using RobotFactory.SharedComponents.Dtos.ApiRequests.Robot.SupplyComponents;

namespace RobotFactory.ComponentSupplier
{
    public class ComponentSupplierFunctions
    {
        private readonly HttpClient _factoryApiHttpClient;

        public ComponentSupplierFunctions(IHttpClientFactory factoryApiHttpClient, IConfiguration configuration)
        {
            _factoryApiHttpClient = factoryApiHttpClient.CreateClient();
            _factoryApiHttpClient.BaseAddress = new Uri(configuration["RobotFactoryApiUri"]);
        }

        [FunctionName(nameof(SendComponentsToTheDatabaseStore))]
        public async Task<IActionResult> SendComponentsToTheDatabaseStore(
            [ActivityTrigger] RobotComponent[] robotComponents,
            ILogger log)
        {
            var requestObject = new SupplyComponentsRequest() { Components = robotComponents };
            var request = new StringContent(
                JsonConvert.SerializeObject(requestObject),
                Encoding.UTF8,
                "application/json");
            var response = await _factoryApiHttpClient.PostAsync("supply-components", request);
            log.LogInformation("Got response from API: {0}", await response.Content.ReadAsStringAsync());

            return new OkObjectResult(response);
        }
    }
}