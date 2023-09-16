using AzureFileUploader.Application.Abstractions.BlobStorage;
using AzureFileUploader.Application.Abstractions.Messaging;
using AzureFileUploader.Domain.Abstractions;

namespace AzureFileUploader.Application.Users.UploadDocument;

public sealed class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand>
{
    private readonly IBlobStorageService _blobStorageService;

    public UploadDocumentCommandHandler(IBlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    public async Task<Result> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Document.FileName)}";

        var blobClient = await _blobStorageService.GetBlobClient(fileName);

        await _blobStorageService.UploadBlobAsync(request.Document, blobClient,
            new Dictionary<string, string>()
            {
                { "email", request.Email }
            });

        return Result.Success();
    }
}