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
    public class WorkforceProfileInputItem
    {
        public int Payload_Id { get; set; }
        public int Company_Id { get; set; }
        public string Sector_Name { get; set; }
        public string Sector_Id { get; set; }
        public int Job_Id { get; set; }
        public string Job_Title { get; set; }
        public string Phraseology { get; set; }
        public int Max_Number_NOCs { get; set; }
        public decimal Min_Similarity_Score { get; set; }
        public string Lang { get; set; }
    }

    public static class AddWorkProfileInputs
    {
        [FunctionName("AddWorkProfileInputs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "dbo.Workforce_Profile_Input", connectionStringSetting: "SqlConnectionStringStage")] IAsyncCollector<WorkforceProfileInputItem> workforceProfileInputItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            WorkforceProfileInputItem workforceProfileInputItem = JsonConvert.DeserializeObject<WorkforceProfileInputItem>(requestBody);

            await workforceProfileInputItems.AddAsync(workforceProfileInputItem);
            await workforceProfileInputItems.FlushAsync();
            List<WorkforceProfileInputItem> workforceProfileInputItemList = new List<WorkforceProfileInputItem> { workforceProfileInputItem };

            return new OkObjectResult(workforceProfileInputItemList);
        }
    }
}
