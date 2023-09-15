using System;
using System.Collections.Generic;
using Mandrill;
using System.Threading.Tasks;
using Mandrill.Models;
using Mandrill.Requests.Messages;

namespace AzureFileUploader.Functions.Services.Email;
internal sealed class EmailService : IEmailService
{
    public async Task<List<EmailResult>> SendAsync(string recipient, string subject, string body)
    {
        var mandrillApiKey = Environment.GetEnvironmentVariable("MandrillApiKey");
        
        var api = new MandrillApi(mandrillApiKey);
        
        var emailMessage = new EmailMessage
        {
            Subject = subject,
            FromEmail = "noreply@cloasys.world",
            To = new[] {new EmailAddress(recipient)},
            Html = body,
            AutoHtml = true
        };
        return await api.SendMessage(new SendMessageRequest(emailMessage));
    }
}

