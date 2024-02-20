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
using Azure.Core;
using Azure.Storage.Blobs.Models;
using System.Reflection.Metadata;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using Azure;

namespace NGen
{

    public static class AddDutyOutputs
    {
        [FunctionName("AddDutyOutputs")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "AddDutyOutputs")] HttpRequest req,
            ILogger log)
        {

            string payload = await new StreamReader(req.Body).ReadToEndAsync();

            string result = TryLoadJson(payload);

            if (result == null)
            {
                log.LogInformation("JSON is well formed");
                var resultjson = JsonConvert.DeserializeObject<MyRequest>(payload);

                string filename = "payload_" + resultjson.payload_id.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".json";

                string env_connstring = Environment.GetEnvironmentVariable("AzureBlobStorageConnectionString");
                string env_container = "dutyoutputs";

                WriteToBlob(env_connstring, env_container, filename, payload);

                return new OkResult();

            }
            else
            {
                log.LogInformation("JSON is badly fomred");

                return new BadRequestObjectResult(result);
            }
        }

        public static void WriteToBlob(string env_connstring, string env_container, string fileName, string content)
        {

            string filePath = $"{fileName}";

            BlobContainerClient containerClient = new(env_connstring, env_container);
            BlobClient blobClient = containerClient.GetBlobClient(filePath);

            blobClient.Upload(BinaryData.FromString(content), overwrite: true);
        }

        static string TryLoadJson(string json)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<MyRequest>(json);
                return null;
            }
            catch (JsonReaderException ex)
            {
                return ex.Message;
            }
        }

        public static async Task<BlobLeaseClient> AcquireBlobLeaseAsync(BlobClient blobClient)
        {
            // Get a BlobLeaseClient object to work with a blob lease
            BlobLeaseClient leaseClient = blobClient.GetBlobLeaseClient();

            Response<BlobLease> response =
                await leaseClient.AcquireAsync(duration: TimeSpan.FromSeconds(30));

            // Use response.Value to get information about the blob lease

            return leaseClient;
        }

        public class MyRequest
        {
            public int payload_id { get; set; }

            public List<Categories> data { get; set; }
        }

        public class Categories
        {
            public string category_label { get; set; }

            public List<Matches> data { get; set; }

        }

        public class Matches
        {
            public string name { get; set; }
            public string Similiarty_Score { get; set; }

        }


    }

}
