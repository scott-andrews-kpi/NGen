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
    public class CompanyJobListAltItem
    {
        public string NOC_Code { get; set; }
        public string NOC_Label { get; set; }
        public string Competency_Type { get; set; }
        public string Competencies { get; set; }

    }
    public static class GetCompanyJobListAlt
    {
        [FunctionName("GetCompanyJobListAlt")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetCompanyJobListAlt")] HttpRequest req,
        ILogger log,
            [Sql(commandText: "GetCompanyJobListAlt", connectionStringSetting: "SqlConnectionString", commandType: System.Data.CommandType.StoredProcedure, parameters: "@lang={Query.lang},@company_id={Query.company_id}")] IAsyncEnumerable<CompanyJobListAltItem> CompanyJobListAlt)
        {
            return new OkObjectResult(CompanyJobListAlt);
        }
    }
}
