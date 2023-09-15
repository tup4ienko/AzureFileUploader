namespace AzureFileUploader.Api.Controllers.User;

public sealed record UploadDocumentRequest(
    string Email,
    IFormFile Document);