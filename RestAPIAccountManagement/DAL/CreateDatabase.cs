using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace RestAPIAccountManagement.DAL
{
    public class CreateDatabase
    {
        /// <summary>
        /// Creates a database if it does not exist
        /// </summary>
        /// <param name="connectionStringWithoutDb">The connection string without the database name</param>
        /// <param name="databaseName">The name of the database to create</param>
        /// <returns>True if the database was created or already exists, otherwise false</returns>
        public async Task<bool> CreateDatabaseAsync(string connectionStringWithoutDb, string databaseName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringWithoutDb))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"CREATE DATABASE IF NOT EXISTS {databaseName}";
                        await command.ExecuteNonQueryAsync();
                    }
                    Console.WriteLine("Database created successfully or already exists.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error creating the database: {e.Message}");
                Console.Error.WriteLine($"Stack Trace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Creates a table if it does not exist
        /// </summary>
        /// <param name="connectionStringWithDb">The connection string with the database name</param>
        /// <returns>True if the table was created or already exists, otherwise false</returns>
        public async Task<bool> CreateTableAsync(string connectionStringWithDb)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringWithDb))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText =
                            "CREATE TABLE IF NOT EXISTS Accounts (Id INT AUTO_INCREMENT PRIMARY KEY, Name VARCHAR(255), Email VARCHAR(255), PasswordHash VARCHAR(255), Role VARCHAR(255))";
                        await command.ExecuteNonQueryAsync();
                    }
                    Console.WriteLine("Table created successfully or already exists.");
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error creating the table: {e.Message}");
                Console.Error.WriteLine($"Stack Trace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Checks if the database exists
        /// </summary>
        /// <param name="connectionStringWithoutDb">The connection string without the database name</param>
        /// <param name="databaseName">The name of the database to check</param>
        /// <returns>True if the database exists, otherwise false</returns>
        public async Task<bool> CheckDatabaseExistsAsync(string connectionStringWithoutDb, string databaseName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringWithoutDb))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{databaseName}'";
                        var result = await command.ExecuteScalarAsync();
                        if (result != null)
                        {
                            Console.WriteLine("Database exists.");
                            return true;
                        }
                        Console.WriteLine("Database does not exist.");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error checking if database exists: {e.Message}");
                Console.Error.WriteLine($"Stack Trace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Checks if the table exists
        /// </summary>
        /// <param name="connectionStringWithDb">The connection string with the database name</param>
        /// <returns>True if the table exists, otherwise false</returns>
        public async Task<bool> CheckTableExistsAsync(string connectionStringWithDb)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionStringWithDb))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = DATABASE() AND table_name = 'Accounts'";
                        var result = await command.ExecuteScalarAsync();
                        if (Convert.ToInt32(result) > 0)
                        {
                            Console.WriteLine("Table exists.");
                            return true;
                        }
                        Console.WriteLine("Table does not exist.");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Error checking if table exists: {e.Message}");
                Console.Error.WriteLine($"Stack Trace: {e.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Starts the creation of the database and table
        /// </summary>
        /// <param name="connectionStringWithDb">The connection string with the database name</param>
        /// <returns>True if the database and table were created or already exist, otherwise false</returns>
        public async Task<bool> StartCreateDatabaseAsync(string connectionStringWithDb)
        {
            // Extract the database name from the connection string
            var databaseName = new MySqlConnectionStringBuilder(connectionStringWithDb).Database;

            // Create a connection string without the database name
            var connectionStringWithoutDb = new MySqlConnectionStringBuilder(connectionStringWithDb)
            {
                Database = null
            }.ToString();

            if (!await CheckDatabaseExistsAsync(connectionStringWithoutDb, databaseName))
            {
                if (!await CreateDatabaseAsync(connectionStringWithoutDb, databaseName))
                {
                    return false;
                }
            }

            if (!await CheckTableExistsAsync(connectionStringWithDb))
            {
                if (!await CreateTableAsync(connectionStringWithDb))
                {
                    return false;
                }
            }

            return true;
        }
    }
}