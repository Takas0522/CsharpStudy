using System;
using System.Text;
using LearnFunctionApp.QueueStorage;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Utf8Json;

namespace LearnFunctionApp
{
    public class TimerFunction
    {
        private readonly IQueueStorageService _service;

        public TimerFunction(
            IQueueStorageService service
        )
        {
            _service = service;
        }

        [FunctionName("TimerFunction")]
        [return: Queue("samplequeue", Connection = "StorageSettings:ConnectionString")]
        public MessageModel Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


            var now = DateTime.Now;
            var data = new MessageModel { SampleMessage = "testMessage", MakeDate = now };
            return data;

            //_service.SendSampleMessage();
        }
    }
}
