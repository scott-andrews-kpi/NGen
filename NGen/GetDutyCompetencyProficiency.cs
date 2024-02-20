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

    public class DutyCompetencyProficiencyItem
    {
        public string Competency_Type { get; set; }
        public string Competency_Name { get; set; }
        public decimal AvgCompetencyLevel { get; set; }

    }
    public static class GetDutyCompetencyProficiency
    {
        [FunctionName("GetDutyCompetencyProficiency")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetDutyCompetencyProficiency")] HttpRequest req,
        ILogger log,
            [Sql(commandText: "GetDutyCompProficiency", connectionStringSetting: "SqlConnectionString", commandType: System.Data.CommandType.StoredProcedure, parameters: "@lang={Query.lang},@competency={Query.competency}")] IAsyncEnumerable<DutyCompetencyProficiencyItem> DutyCompProficiency)
        {
            return new OkObjectResult(DutyCompProficiency);
        }
    }
}
