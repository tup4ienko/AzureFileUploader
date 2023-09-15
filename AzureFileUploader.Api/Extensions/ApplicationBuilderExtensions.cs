using AzureFileUploader.Api.Middleware;

namespace AzureFileUploader.Api.Extensions;

public static class ApplicationBuilderExtensions
{

    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}