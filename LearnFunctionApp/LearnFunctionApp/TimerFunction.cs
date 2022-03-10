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
        public void Run(
            [TimerTrigger("0 */5 * * * *")]TimerInfo myTimer,
            [Queue("samplequeue", Connection = "StorageSettings:ConnectionString")] ICollector<MessageModel> collector,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // cf: https://stackoverflow.com/questions/52199336/multiple-messages-as-azure-function-ouput-parameter

            // Queue10å¬çÏÇÈ
            var now = DateTime.Now;
            collector.Add(new MessageModel { SampleMessage = "testMessage1", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage2", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage3", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage4", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage5", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage6", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage7", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage8", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage9", MakeDate = now });
            collector.Add(new MessageModel { SampleMessage = "testMessage10", MakeDate = now });

            //_service.SendSampleMessage();
        }
    }
}
