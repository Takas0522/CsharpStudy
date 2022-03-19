using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace LearnFunctionApp
{
    public static class QueueFunction
    {
        [FunctionName("QueueFunction")]
        public static void Run([QueueTrigger("samplequeue", Connection = "StorageSettings:ConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            // 5回施行失敗したら[Queue名]-posion Queueストレージにストアされるテスト
            //throw new Exception("Appliction Error");
        }
    }
}
