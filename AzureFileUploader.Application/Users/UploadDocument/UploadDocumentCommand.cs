using AzureFileUploader.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace AzureFileUploader.Application.Users.UploadDocument;

public record UploadDocumentCommand(
    string Email,
    IFormFile Document) : ICommand;