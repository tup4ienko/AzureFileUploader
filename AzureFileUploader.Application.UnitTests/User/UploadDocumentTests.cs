using Azure.Storage.Blobs;
using AzureFileUploader.Application.Abstractions.BlobStorage;
using AzureFileUploader.Application.Users.UploadDocument;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AzureFileUploader.Application.UnitTests.User;

public class UploadDocumentTests
{
    [Fact]
    public async Task Handle_UploadsDocumentSuccessfully()
    {
        // Arrange
        var email = "test@example.com";
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.FileName).Returns("test.doc");
        var command = new UploadDocumentCommand(email, formFile.Object);

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
                It.Is<Dictionary<string, string>>(data => data["email"] == email)),
            Times.Once);
    }
}