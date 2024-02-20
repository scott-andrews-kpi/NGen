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
using Microsoft.Data.SqlClient;
using System.Data;

namespace NGen
{
    public class Item
    {
        public string Noc_Code { get; set; }
        public string Noc_Label { get; set; }
        public string Competencies { get; set; }

    }

    public class HeatMapData
    {
        [FunctionName("GetHeatMapData")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GetHeatMapData")] HttpRequest req,
            ILogger log)
        {
            // Read Json body
            string noc_string = await new StreamReader(req.Body).ReadToEndAsync();

            // Clean Json for database processing
            noc_string = noc_string.Replace("\"Noc_Code\": ", "");
            noc_string = noc_string.Replace("[", "'");
            noc_string = noc_string.Replace("]", "'");
            noc_string = noc_string.Replace("\"", "");
            noc_string = noc_string.Replace("\r", "");
            noc_string = noc_string.Replace("\n", "");
            noc_string = noc_string.Replace("\t", "");
            noc_string = noc_string.Replace(" ", "");
            log.LogInformation(noc_string);

            string lang = req.Query["lang"];

            string sql_cmd = $"exec GetHeatMap '{lang}', " + noc_string;
            log.LogInformation(sql_cmd);


            using (SqlConnection connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnectionString")))
            {
                connection.Open();

                List<Item> items = null;

                SqlCommand command = new SqlCommand(sql_cmd, connection);
                var dataReader = command.ExecuteReader();
                items = GetList<Item>(dataReader);

                return new OkObjectResult(items);

            }

        }

        private static List<T> GetList<T>(IDataReader reader)
        {
            List<T> list = new List<T>();

            while(reader.Read())
            {
                var type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                foreach (var prop  in type.GetProperties())
                {
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(reader[prop.Name].ToString(),propType));
                }
                list.Add(obj);
            }

            return list;
        }

    }



 
}
