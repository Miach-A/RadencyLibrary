using RadencyLibrary.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(cfg => cfg.SwaggerEndpoint("v1/swagger.json", "Library v1"));

app.UseAuthorization();

app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.SeedLibrary();
}
app.Run();
