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
    public class WorkforceProfileOutputItem
    {
        public int Payload_Id { get; set; }
        public string Noc_Code { get; set; }
        public string Lead_Statement { get; set; }
        public string Main_Duties { get; set; }
        public string Job_Titles { get; set; }
        public string Lang { get; set; }
    }

    public static class AddWorkProfileOutputs
    {
        [FunctionName("AddWorkProfileOutputs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "dbo.Workforce_Profile_Output", connectionStringSetting: "SqlConnectionStringStage")] IAsyncCollector<WorkforceProfileOutputItem> workforceProfileOutputItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            WorkforceProfileOutputItem workforceProfileOutputItem = JsonConvert.DeserializeObject<WorkforceProfileOutputItem>(requestBody);

            await workforceProfileOutputItems.AddAsync(workforceProfileOutputItem);
            await workforceProfileOutputItems.FlushAsync();
            List<WorkforceProfileOutputItem> workforceProfileOutputItemList = new List<WorkforceProfileOutputItem> { workforceProfileOutputItem };

            return new OkObjectResult(workforceProfileOutputItemList);
        }
    }
}