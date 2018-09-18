using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;
using FunctionColorApp.Models;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace FunctionColorApp
{
    public static class ColorOperations
    {
        [FunctionName("GetAll")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "colors")]HttpRequest req,
            [Table("colorsTable", Connection = "MyTable")]CloudTable inTable,
            TraceWriter log)
        {
            log.Info("Petició de tots els colors.");                       
            return new OkObjectResult(inTable.GetAllColorsFromTable());
        }


        [FunctionName("GetColor")]
        public static IActionResult GetColor([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "colors/{rgb}/{idioma}")]HttpRequest req,
                                             [Table("colorsTable", Connection = "MyTable")]CloudTable inTable,
                                              string rgb, string idioma,
                                              TraceWriter log)
        {
            log.Info($"Petició del color {rgb} en {idioma}");

            var item = inTable.GetColorFromTable(rgb, idioma);
            return new OkObjectResult(item);
        }

        [FunctionName("GetColorTranslations")]
        public static IActionResult GetColorTranslations([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "colors/{rgb}")]HttpRequest req,
                                     [Table("colorsTable", Connection = "MyTable")]CloudTable table,
                                      string rgb, 
                                      TraceWriter log)
        {
            log.Info($"Petició del color {rgb}");

            var colors = table.GetColorTranslationFromTable(rgb);

            return new OkObjectResult(colors);
        }


        [FunctionName("CreateColor")]
        public static async Task<IActionResult> CreateColor([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "colors")]HttpRequestMessage req, 
                                                            [Table("colorsTable", Connection = "MyTable")]CloudTable table, 
                                                            TraceWriter log, 
                                                            ExecutionContext context)
        {
         
            try
            {
                var json = await req.Content.ReadAsStringAsync();
                var color = JsonConvert.DeserializeObject<Color>(json);
                table.AddOrUpdateColorToTable(color);
                
                log.Info($"Color creat");
                return new CreatedResult("GetColor", color);                
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("Can't create color: " + e.Message + " ");
            }
        }
    }
}
