using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace FunctionColorApp
{
    /// <summary>
    /// Funció que s'executa quan es rep un missatge a la cua
    /// </summary>
    public static class ColorStatisticsFunction
    {
        [FunctionName("ColorStatisticsFunction")]
        public static void Run([QueueTrigger("color-added-queue", Connection = "MyTable")]string myQueueItem, TraceWriter log)
        {
            log.Info($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
