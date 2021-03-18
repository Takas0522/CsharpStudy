using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Utf8Json;

namespace LearnFunctionApp.QueueStorage
{
    public class QueueStorageService : IQueueStorageService
    {
        private readonly string _connectionString;
        private readonly QueueClient _client;

        public QueueStorageService(IOptions<StorageSettings> setting)
        {
            _connectionString = setting.Value.ConnectionString;
            _client = new QueueClient(_connectionString, "samplequeue");
        }

        public void SendSampleMessage()
        {
            _client.CreateIfNotExists();
            if (_client.Exists())
            {
                var now = DateTime.Now;
                var data = new MessageModel { SampleMessage = "testMessage", MakeDate = now };
                var enc = Encoding.UTF8;
                var json = JsonSerializer.ToJsonString<MessageModel>(data);
                var message = Convert.ToBase64String(enc.GetBytes(json));
                _client.SendMessage(message);
            }
        }
    }
}
