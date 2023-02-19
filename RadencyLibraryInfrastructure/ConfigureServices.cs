using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RadencyLibraryInfrastructure.Persistence;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<LibraryDbContext>(options =>
           options.UseInMemoryDatabase("LibraryDatabase"));

        return services;
    }
}
