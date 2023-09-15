using AzureFileUploader.Application.Abstractions.BlobStorage;
using AzureFileUploader.Infrastructure.BlobStorage;
using Microsoft.Extensions.DependencyInjection;

namespace AzureFileUploader.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddTransient<IBlobStorageService, BlobStorageService>();

        return services;
    }
}