using AutoMapper;
using FluentValidation;
using MediatR;
using RadencyLibrary.Common.Behaviours;
using RadencyLibrary.Common.Mappings;
using System.Reflection;


namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var maperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BookProfile>();
        });

        maperConfig.AssertConfigurationIsValid();
        var mapper = maperConfig.CreateMapper();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(cfg =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            cfg.IncludeXmlComments(xmlPath);
        });
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
