using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RobotFactory.DataLayer.Models;

namespace RobotFactory.ComponentSuplierFunctions
{
    public class DurableFunctionsEntityHttpCSharp
    {
        private readonly HttpClient _factoryApiHttpClient;

        public DurableFunctionsEntityHttpCSharp(HttpClient factoryApiHttpClient, IConfiguration configuration)
        {
            _factoryApiHttpClient = factoryApiHttpClient;
            _factoryApiHttpClient.BaseAddress = new Uri(configuration["RobotFactoryApiUri"]);
        }

        [FunctionName(nameof(SendComponentsToTheDatabaseStore))]
        public async Task<IActionResult> SendComponentsToTheDatabaseStore(
            [ActivityTrigger] RobotComponent[] requestedComponent,
            ILogger log)
        {
            var response = await _factoryApiHttpClient.GetAsync("health-check");
            log.LogCritical("Got response from API: {0}", await response.Content.ReadAsStringAsync());

            return new OkObjectResult(response);
        }
    }
}