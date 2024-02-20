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
    public class DutyFeedbackItem
    {
        public int Payload_Id { get; set; }
        public string Duty_Selection { get; set; }
        public string Description { get; set; }
        public string Lang { get; set; }
    }

    public static class AddDutyFeedback
    {
        [FunctionName("AddDutyFeedback")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "dbo.Duty_Feedback", connectionStringSetting: "SqlConnectionStringStage")] IAsyncCollector<DutyFeedbackItem> dutyFeedbackItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            DutyFeedbackItem dutyFeedbackItem = JsonConvert.DeserializeObject<DutyFeedbackItem>(requestBody);

            await dutyFeedbackItems.AddAsync(dutyFeedbackItem);
            await dutyFeedbackItems.FlushAsync();
            List<DutyFeedbackItem> dutyFeedbackItemList = new List<DutyFeedbackItem> { dutyFeedbackItem };

            return new OkObjectResult(dutyFeedbackItemList);
        }
    }
}
