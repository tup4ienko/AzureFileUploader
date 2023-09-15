using FluentValidation;
using FluentValidation.Validators;

namespace AzureFileUploader.Application.Users.UploadDocument;

public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(c => c.Email).NotNull()
            .EmailAddress(EmailValidationMode.Net4xRegex).WithMessage("Invalid email address.");

        RuleFor(x => x.Document)
            .NotNull()
            .Must(x =>
                x.ContentType.Equals("application/msword") ||
                x.ContentType.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
            .WithMessage("File type is not allowed. Only .doc and .docx files are accepted.");
    }
}