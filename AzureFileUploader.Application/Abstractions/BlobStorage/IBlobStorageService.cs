using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace AzureFileUploader.Application.Abstractions.BlobStorage;


public interface IBlobStorageService
{
    Task UploadBlobAsync(IFormFile formFile, BlobClient blobClient, IDictionary<string, string> metadata = null);
    Task<BlobContainerClient> GetBlobContainerClient();
    Task<BlobClient> GetBlobClient(string blobName);
}