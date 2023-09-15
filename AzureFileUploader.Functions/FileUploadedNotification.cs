using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AzureFileUploader.Functions.Services.BlobStorage;
using AzureFileUploader.Functions.Services.Email;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace AzureFileUploader.Functions
{
    public class FileUploadedNotification
    {
        private readonly IEmailService _emailService;
        private readonly IBlobStorage _blobStorage;

        public FileUploadedNotification(IEmailService emailService, IBlobStorage blobStorage)
        {
            _emailService = emailService;
            _blobStorage = blobStorage;
        }

        [FunctionName("FileUploadedNotification")]
        public async Task RunAsync(
            [BlobTrigger("az-document-container/{name}", Connection = "AzureWebJobsStorage")]
            Stream myBlob,
            string name,
            IDictionary<string, string> metaData,
            ILogger log)
        {
            try
            {
                var recipient = metaData["email"];
                
                log.LogInformation("Start processing file {0}", name);
                var sasUri = await _blobStorage.GetBlobUrl(name);
            
                var subject = "Your File Successfully Uploaded";
            
                var bodyHtml = $"""
                                <p>We are pleased to inform you that your file has been successfully uploaded to our system. You can now access it using the link provided below. Please note that this link will be active for the next hour.</p>

                                <p>{sasUri}</p>
                                """;

                var emailResults = await _emailService.SendAsync(recipient, subject, bodyHtml);
                
                foreach (var result in emailResults)
                {
                    var logMessage = $"Email: {result.Email}, Id: {result.Id}, Status: {result.Status}, RejectReason: {result.RejectReason}";
                    log.LogInformation(logMessage);
                }
                log.LogInformation("File {0} processed successfully", name);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                throw;
            }
            
        }
    }
}