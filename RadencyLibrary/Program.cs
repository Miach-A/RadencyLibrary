using Microsoft.AspNetCore.Diagnostics;
using RadencyLibrary.Common.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices(builder.Configuration);


    var app = builder.Build();
    app.UseExceptionHandler(c => c.Run(async context =>
    {
        var exception = context.Features
            .Get<IExceptionHandlerPathFeature>()!
            .Error;
        var response = new { error = exception.Message };
        await context.Response.WriteAsJsonAsync(response);
    }));

    app.UseHttpLogging();
    app.UseSwagger();
    app.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("v1/swagger.json", "Library v1"));

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();
    app.SeedLibrary();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
