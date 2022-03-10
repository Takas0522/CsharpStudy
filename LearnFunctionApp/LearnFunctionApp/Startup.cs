using LearnFunctionApp.QueueStorage;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(LearnFunctionApp.Startup))]
namespace LearnFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.Configure<StorageSettings>(configuration.GetSection("StorageSettings"));

            services.AddTransient<IQueueStorageService, QueueStorageService>();
        }
    }
}
