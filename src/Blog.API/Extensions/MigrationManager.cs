using System;
using Blog.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Blog.API.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            Log.Information("Migrating database...");

            using(var scope = host.Services.CreateScope())
            {
                using(var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                        Seed(context, scope.ServiceProvider);
                        Log.Information("Database migration successful");
                    }
                    catch (Exception ex)
                    {
                        Log.Information("Error migrating database: " + ex + "\n Message: " + ex.Message + "\n Inner exception: " + ex.InnerException);
                    }
                }
            }

            return host;
        }

        private static void Seed(ApplicationDbContext context, IServiceProvider provider)
        {
            // Implement seed
        }
    }
}
