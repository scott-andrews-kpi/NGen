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
    public class CompanyJobListItem
    {
        public string NOC_Code { get; set; }
        public string NOC_Label { get; set; }
        public string Competency_Type { get; set; }
        public string Competency_Sub_Category { get; set; }
        public string Competency_Name { get; set; }
        public int ? Competency_Level { get; set; }
        public int ? Future_Competency_Level { get; set; }

    }
    public static class GetCompanyJobList
    {
        [FunctionName("GetCompanyJobList")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetCompanyJobList")] HttpRequest req,
        ILogger log,
            [Sql(commandText: "GetCompanyJobList", connectionStringSetting: "SqlConnectionString", commandType: System.Data.CommandType.StoredProcedure, parameters: "@lang={Query.lang},@company_id={Query.company_id}")] IAsyncEnumerable<CompanyJobListItem> CompanyJobList)
        {
            return new OkObjectResult(CompanyJobList);
        }
    }
}
