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
    public class Guide
    {
        public string Competency_Type { get; set; }
        public string Competency_Sub_Category { get; set; }
        public string Competency_Sub_Category_Description { get; set; }
        public string Competencies { get; set; }
    }
    public static class GuideDescriptions
    {
        [FunctionName("GuideDescriptions")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GuideDescriptions")] HttpRequest req,
            ILogger log,
            [Sql(commandText: "GetGuideDescriptions", connectionStringSetting: "SqlConnectionString", commandType: System.Data.CommandType.StoredProcedure, parameters: "@lang={Query.lang}")]
        IAsyncEnumerable<Guide> GuideDescriptions)
        {
            return new OkObjectResult(GuideDescriptions);
        }
    }
}