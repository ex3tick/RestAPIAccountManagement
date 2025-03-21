using MySql.Data.MySqlClient;
using RestAPIAccountManagement.Hashing;
using RestAPIAccountManagement.Models.UserModel;

namespace RestAPIAccountManagement.DAL;

public class AccountDAL
{
    
    /// <summary>
    ///  This property is used to set the connection string and is set in the Program.cs
    /// </summary>
    public static string ConnectionString { get; set; }
   
    /// <summary>
    ///  This method is used to select the account by email and password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> SelectAccountByEmailAndPassword(string email, string password)
    {
        string passwordHash = HashHelper.HashWithSaltAndPepper(password).Password;
        
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM Accounts WHERE Email = '{email}' AND PasswordHash = '{passwordHash}'";
                    var result = await command.ExecuteReaderAsync();
                    if (!result.HasRows)
                    {
                        Console.WriteLine("Account does not exist.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Account exists.");
                        return true;
                    }
                }


            }

        }
        catch (Exception e)
        {
            Console.WriteLine("Error selecting account by email and password");
            Console.WriteLine(e);
            throw;
        }

        return false;
    }
    
    /// <summary>
    ///  This method is used to select the account by username and password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<bool> SelectAccountByUsernameAndPassword(string username, string password)
    {
        string passwordHash = HashHelper.HashWithSaltAndPepper(password).Password;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"SELECT * FROM Accounts WHERE Name = '{username}' AND PasswordHash = '{passwordHash}'";
                    var result = await command.ExecuteReaderAsync();
                    if (!result.HasRows)
                    {
                        Console.WriteLine("Account does not exist.");
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Account exists.");
                        return true;
                    }
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error selecting account by username and password");
            Console.WriteLine(e);
            throw;
        }
        
    }
    
    /// <summary>
    ///  `This method is used to select the account by email
    /// </summary>
    /// <param name="userModel"> </param>
    /// <returns>a</returns>
    public async Task<bool> InsertUser(UserModel userModel)
    {
        userModel.PasswordHash = HashHelper.HashWithSaltAndPepper(userModel.PasswordHash).Password;
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"INSERT INTO Accounts (Name, Email, PasswordHash, Role) VALUES ('{userModel.Name}', '{userModel.Email}', '{userModel.PasswordHash}', '{userModel.Role}')";
                    await command.ExecuteNonQueryAsync();
                    Console.WriteLine("Account inserted");
                    return true;
                }
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine("Error inserting account");
            Console.WriteLine(e);
            throw;
        }
    }
    public async Task<bool> UpdateAccount()
    {

        return false;
    }
    public async Task<bool> DeleteAccount()
    {

        return false;
    }
    
    
    
}