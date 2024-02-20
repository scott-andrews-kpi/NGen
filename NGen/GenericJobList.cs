using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace NGen
{
    public class Job
    {
        public string Noc_Label { get; set; }
        public string Noc_Code { get; set; }
        public string Lead_Statement { get; set; }
        public string Main_Duties { get; set; }
        public string Job_Titles { get; set; }

    }
    public static class GenericJobList
    {
        [FunctionName("GenericJobList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GenericJobList")] HttpRequest req,
        ILogger log,
            [Sql(commandText: "GetGenericJobList", connectionStringSetting: "SqlConnectionString", commandType: System.Data.CommandType.StoredProcedure, parameters: "@lang={Query.lang}")] IAsyncEnumerable<Job> GenericJobList)
        {
            return new OkObjectResult(GenericJobList);
        }
    }
}
