using AzureFileUploader.Functions.Services.BlobStorage;
using AzureFileUploader.Functions.Services.Email;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AzureFileUploader.Functions.Startup))]

namespace AzureFileUploader.Functions
{
    internal class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IBlobStorage, BlobStorage>();
        }
    }
}