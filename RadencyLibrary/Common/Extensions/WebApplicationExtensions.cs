using RadencyLibraryInfrastructure.Extensions;
using RadencyLibraryInfrastructure.Persistence;

namespace RadencyLibrary.Common.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void SeedLibrary(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                //context.Database.EnsureCreated();
                context.SeedLibrary();
            }
        }
    }
}
