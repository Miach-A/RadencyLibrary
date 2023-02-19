using AutoMapper;
using RadencyLibrary.Common.Mappings;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        var maperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<BookProfile>();
        });

        maperConfig.AssertConfigurationIsValid();
        var mapper = maperConfig.CreateMapper();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
