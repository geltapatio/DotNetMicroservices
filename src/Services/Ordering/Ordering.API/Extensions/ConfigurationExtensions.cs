using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ConfigurationManager MigrateDatabase<TContext>(this ConfigurationManager configurationManager, IServiceProvider services,
                                            Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
                int retryForAvailability = retry.Value;
                var logger = services.GetRequiredService<ILogger<TContext>>();
                using var scope = services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<TContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    InvokeSeeder(seeder, context, services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                    if (retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(configurationManager, services, seeder, retryForAvailability);
                    }
                }

                return configurationManager;
            }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder,
                                                       TContext context,
                                                       IServiceProvider services)
                                                       where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }

    }
}
