using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureFileUploader.Application.Abstractions.BlobStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AzureFileUploader.Infrastructure.BlobStorage;

public sealed class BlobStorageService : IBlobStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _containerName = "az-document-container";

    public BlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task UploadBlobAsync(IFormFile formFile, BlobClient blobClient, 
        IDictionary<string, string> metadata = null)
    {
        using var memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        BlobUploadOptions options = new BlobUploadOptions() { Metadata = metadata };
        await blobClient.UploadAsync(memoryStream, options, default);
    }

    public async Task<BlobContainerClient> GetBlobContainerClient()
    {
        var container = new BlobContainerClient(
            _configuration["StorageConnectionString"], _containerName);
        await container.CreateIfNotExistsAsync();

        return container;
    }

    public async Task<BlobClient> GetBlobClient(string blobName)
    {
        var container = await GetBlobContainerClient();

        var blobClient = container.GetBlobClient(blobName);

        return blobClient;
    }
}