using AutoMapper;
using FluentValidation;
using MediatR;
using RadencyLibrary.Common.Behaviours;
using RadencyLibrary.Common.Mappings;
using RadencyLibrary.CQRS.Base;
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
        services.AddSingleton<IMapper>(mapper);

        services.AddHttpLogging(cfg =>
        {
            cfg.LoggingFields = AspNetCore.HttpLogging.HttpLoggingFields.All;
            cfg.RequestHeaders.Add("Referer");
            cfg.RequestHeaders.Add("sec-ch-ua");
            cfg.RequestHeaders.Add("sec-ch-ua-mobile");
            cfg.RequestHeaders.Add("sec-ch-ua-platform");
            cfg.RequestHeaders.Add("Sec-Fetch-Site");
            cfg.RequestHeaders.Add("Sec-Fetch-Mode");
            cfg.RequestHeaders.Add("Sec-Fetch-Dest");

        });

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

        services.AddTransient(typeof(Response<,>));

        return services;
    }
}
