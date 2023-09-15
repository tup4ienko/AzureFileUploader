using AzureFileUploader.Application.Users.UploadDocument;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AzureFileUploader.Api.Controllers.User;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly ISender _sender;

    public UserController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost("/upload-document")]
    public async Task<IActionResult> UploadDocument(
        [FromForm] UploadDocumentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UploadDocumentCommand(
            request.Email, request.Document);
        
        var result = await _sender.Send(command, cancellationToken);
        
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
