using System.Collections.Generic;
using System.Threading.Tasks;
using Mandrill.Models;

namespace AzureFileUploader.Functions.Services.Email;

public interface IEmailService
{
    Task<List<EmailResult>> SendAsync(string recipient, string subject, string body);
}