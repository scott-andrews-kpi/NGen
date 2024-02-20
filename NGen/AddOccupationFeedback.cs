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
    public class OccupationFeedbackItem
    {
        public int Payload_Id { get; set; }
        public string Feedback_Selection { get; set; }
        public string Description { get; set; }
        public string Lang { get; set; }
    }

    public static class AddOccupationFeedback
    {
        [FunctionName("AddOccupationFeedback")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "dbo.Occupation_Feedback", connectionStringSetting: "SqlConnectionStringStage")] IAsyncCollector<OccupationFeedbackItem> occupationFeedbackItems)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            log.LogInformation(requestBody);

            OccupationFeedbackItem occupationFeedbackItem = JsonConvert.DeserializeObject<OccupationFeedbackItem>(requestBody);

            await occupationFeedbackItems.AddAsync(occupationFeedbackItem);
            await occupationFeedbackItems.FlushAsync();
            List<OccupationFeedbackItem> occupationFeedbackItemList = new List<OccupationFeedbackItem> { occupationFeedbackItem };

            return new OkObjectResult(occupationFeedbackItemList);
        }
    }
}
