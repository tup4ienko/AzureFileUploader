using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace AzureFileUploader.Functions.Services.BlobStorage;

internal sealed class BlobStorage : IBlobStorage
{
    private readonly string _containerName = "az-document-container";

    public async Task<string> GetBlobUrl(string fileName)
    {
        var container = await GetBlobContainerClient();

        var blob = container.GetBlobClient(fileName);

        BlobSasBuilder blobSasBuilder = new()
        {
            BlobContainerName = blob.BlobContainerName,
            BlobName = blob.Name,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            Protocol = SasProtocol.Https,
            Resource = "b"
        };

        blobSasBuilder.SetPermissions(BlobAccountSasPermissions.Read);
        return blob.GenerateSasUri(blobSasBuilder).ToString();
    }

    private async Task<BlobContainerClient> GetBlobContainerClient()
    {
        var storageConnectionString = Environment.GetEnvironmentVariable("StorageConnectionString");

        var container = new BlobContainerClient(storageConnectionString, _containerName);

        await container.CreateIfNotExistsAsync();

        return container;
    }
}