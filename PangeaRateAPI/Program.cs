using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(a => a.Run(async context =>
{
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var exception = exceptionHandlerPathFeature.Error;

    var result = JsonSerializer.Serialize(new { error = exception.Message });
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(result);
}));

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
