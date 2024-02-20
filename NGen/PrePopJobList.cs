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
using Newtonsoft.Json.Linq;

namespace NGen
{
    public class PrePopJob
    {
        public string Noc_Label { get; set; }
        public string Noc_Code { get; set; }
        public string Lead_Statement { get; set; }
        public string Main_Duties { get; set; }
        public string Job_Titles { get; set; }
    }

    public static class PrePopJobList
    {
        [FunctionName("PrePopJobList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "PrePopJobList")] HttpRequest req,
        ILogger log,
             [Sql(commandText: "GetPrePopJobList", connectionStringSetting: "SqlConnectionString", commandType: System.Data.CommandType.StoredProcedure, parameters: "@lang={Query.lang},@sector_id={Query.sector_id}")] IAsyncEnumerable<PrePopJob> PrePopJobList)
        {
            return new OkObjectResult(PrePopJobList);
        }
    }
}