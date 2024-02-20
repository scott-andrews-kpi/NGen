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
    public class DutyCompetencyInputItem
    {
        public int Payload_Id { get; set; }
        public int Company_Id { get; set; }
        public string Noc_Code { get; set; }
        public string Duty_Description { get; set; }
        public int Max_Number_Competencies { get; set; }
        public decimal Min_Similarity_Score { get; set; }
        public string Lang { get; set; }
    }
    public static class AddDutyInputs
    {
        [FunctionName("AddDutyInputs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "dbo.Duty_Competency_Input", connectionStringSetting: "SqlConnectionStringStage")] IAsyncCollector<DutyCompetencyInputItem> dutyCompetencyInputItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            DutyCompetencyInputItem dutyCompetencyInputItem = JsonConvert.DeserializeObject<DutyCompetencyInputItem>(requestBody);

            await dutyCompetencyInputItems.AddAsync(dutyCompetencyInputItem);
            await dutyCompetencyInputItems.FlushAsync();
            List<DutyCompetencyInputItem> dutyCompetencyInputItemList = new List<DutyCompetencyInputItem> { dutyCompetencyInputItem };

            return new OkObjectResult(dutyCompetencyInputItemList);
        }
    }
}