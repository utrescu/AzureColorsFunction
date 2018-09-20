using System;
using System.Threading.Tasks;
using FunctionColorApp.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;

namespace FunctionColorApp
{
    /// <summary>
    /// Funció que s'executa quan es rep un missatge a la cua
    /// </summary>
    public static class ColorStatisticsFunction
    {
        [FunctionName("ColorStatisticsFunction")]
        public static async Task Statistics(
            [QueueTrigger("color-added-queue", Connection = "MyTable")]Color myQueueItem,
            [Table("colorsStatisticsTable", Connection = "MyTable")]CloudTable table, 
            TraceWriter log)
        {
             var idioma = myQueueItem.Traduccio.Idioma;
            log.Info($"Recompte: {idioma}");
           
            // Agafa la quantitat actual de colors
            var quantitat =  table.GetIdiomQuantitatFromTable(idioma); 

            var item = new IdiomItem(idioma) {
                quantitat = quantitat + 1
            };
            await table.AddOrUpdateStatisticsToTable(item);
            log.Info($"{idioma}: {item.quantitat}");
        }
    }
}
