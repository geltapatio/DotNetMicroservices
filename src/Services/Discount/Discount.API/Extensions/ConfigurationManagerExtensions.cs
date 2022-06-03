using Npgsql;

namespace Discount.API.Extensions
{
    public static class ConfigurationManagerExtensions
    {
        public static ConfigurationManager MigrateDatabase(this ConfigurationManager configurationManager, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

             try
            {
                var connectionString = configurationManager.GetValue<string>("DatabaseSettings:ConnectionString");
                using var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                command.ExecuteNonQuery();
            }
            catch
            {

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    System.Threading.Thread.Sleep(2000);
                    MigrateDatabase(configurationManager, retryForAvailability);
                }
            }

            return configurationManager;
        }
    }
}