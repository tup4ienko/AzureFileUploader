using AzureFileUploader.Api.Controllers.User;
using AzureFileUploader.Api.Extensions;
using AzureFileUploader.Application;
using AzureFileUploader.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddCors(opt => 
{
    opt.AddPolicy("CorsPolicy", policy => 
    {
        policy
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173");
    });
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();