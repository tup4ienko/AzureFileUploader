using Azure.Storage.Blobs;
using AzureFileUploader.Application.Abstractions.BlobStorage;
using AzureFileUploader.Application.Users.UploadDocument;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AzureFileUploader.Application.UnitTests.User;

public class UploadDocumentTests
{
    private const string FileName = "test.docx";
    private const string Email = "test@example.com";

    [Fact]
    public async Task Handle_UploadsDocumentSuccessfully()
    {
        // Arrange
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.FileName).Returns(FileName);
        var command = new UploadDocumentCommand(Email, formFile.Object);

        var blobStorageServiceMock = new Mock<IBlobStorageService>();
        blobStorageServiceMock.Setup(service => service.GetBlobClient(It.IsAny<string>()))
            .ReturnsAsync(new Mock<BlobClient>().Object);

        var handler = new UploadDocumentCommandHandler(blobStorageServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        blobStorageServiceMock.Verify(
            service => service.UploadBlobAsync(
                It.IsAny<IFormFile>(),
                It.IsAny<BlobClient>(),
                It.Is<Dictionary<string, string>>(data => data["email"] == Email)),
            Times.Once);
    }
}