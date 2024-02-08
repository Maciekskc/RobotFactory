using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        public async Task<HttpStatusCode> SendComponentsToTheDatabaseStore(
            [ActivityTrigger] RobotComponent[] robotComponents,
            ILogger log)
        {
            var requestObject = new SupplyComponentsRequest() { Components = robotComponents };
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
            };
            log.LogInformation(JsonSerializer.Serialize(requestObject, options));
            var request = new StringContent(
                JsonSerializer.Serialize(requestObject, options),
                Encoding.UTF8,
                "application/json");
            var response = await _factoryApiHttpClient.PostAsync("supply-components", request);
            log.LogInformation("Got response from API: {0}", await response.Content.ReadAsStringAsync());

            return response.StatusCode;
        }
    }
}