using AzureFileUploader.Functions.Services.BlobStorage;
using AzureFileUploader.Functions.Services.Email;
using Mandrill.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace AzureFileUploader.Functions.UnitTests;

public class FileUploadedNotificationTests
{
    private const string FileName = "test.doc";
    private const string Email = "test@example.com";

    [Fact]
    public async Task RunAsync_SendsEmailSuccessfully()
    {
        // Arrange
        var emailServiceMock = new Mock<IEmailService>();
        emailServiceMock
            .Setup(service => service.SendAsync(Email, "Your File Successfully Uploaded", It.IsAny<string>()))
            .ReturnsAsync(new List<EmailResult> { new EmailResult { Email = Email, Id = "123", Status = EmailResultStatus.Sent } });

        var blobStorageMock = new Mock<IBlobStorage>();
        blobStorageMock.Setup(storage => storage.GetBlobUrl(FileName)).ReturnsAsync("https://example.com/sasuri");

        var loggerMock = new Mock<ILogger>();

        var function = new FileUploadedNotification(emailServiceMock.Object, blobStorageMock.Object);

        var blobStream = new MemoryStream();
        var metaData = new Dictionary<string, string> { { "email", Email } };

        // Act
        await function.RunAsync(blobStream, FileName, metaData, loggerMock.Object);

        // Assert
        blobStorageMock.Verify(storage => storage.GetBlobUrl(FileName), Times.Once);
        emailServiceMock.Verify(service => service.SendAsync(Email, "Your File Successfully Uploaded", It.IsAny<string>()), Times.Once);
    }
}
