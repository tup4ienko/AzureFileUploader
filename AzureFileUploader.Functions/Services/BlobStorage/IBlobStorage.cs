using System.Threading.Tasks;

namespace AzureFileUploader.Functions.Services.BlobStorage;

public interface IBlobStorage
{
    Task<string> GetBlobUrl(string fileName);
}