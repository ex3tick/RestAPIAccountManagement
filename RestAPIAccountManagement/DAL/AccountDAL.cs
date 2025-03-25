using MySql.Data.MySqlClient;
using RestAPIAccountManagement.Hashing;
using RestAPIAccountManagement.Models.UserModel;

namespace RestAPIAccountManagement.DAL;

/// <summary>
/// Data Access Layer for handling all account-related database operations
/// </summary>
public class AccountDAL
{
    /// <summary>
    /// Gets or sets the database connection string (configured in Program.cs)
    /// </summary>
    public static string ConnectionString { get; set; }
    
    private static readonly HashHelper HashHelper = new HashHelper();
   
    /// <summary>
    /// Authenticates a user by email and password
    /// </summary>
    /// <param name="email">User's email address</param>
    /// <param name="password">Plaintext password</param>
    /// <returns>True if credentials are valid, otherwise false</returns>
    public async Task<bool> SelectAccountByEmailAndPassword(string email, string password)
    {
        UserModel account = await SelectAccountByEmail(email);
        if (account != null)
        {
          if(HashHelper.ValliedatePassword(password, account.Salt, account.PasswordHash))
          {
              return true;
          }
        }
        return false;
    }
    
    /// <summary>
    /// Authenticates a user by username and password
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">Plaintext password</param>
    /// <returns>True if credentials are valid, otherwise false</returns>
    public async Task<bool> SelectAccountByUsernameAndPassword(string username, string password)
    {
        UserModel account = await SelectAccountByUsername(username);
        if (account != null)
        {
            if(HashHelper.ValliedatePassword(password, account.Salt, account.PasswordHash))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Inserts a new account into the database
    /// </summary>
    /// <param name="account">UserModel with account details</param>
    /// <returns>True if insertion succeeds, throws exception on failure</returns>
    public async Task<bool> InsertAccount(UserModel account)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO Accounts (Username, LastName, FirstName, Email, PasswordHash, Role, Salt) VALUES ('{account.Username}', '{account.LastName}', '{account.FirstName}', '{account.Email}', '{account.PasswordHash}', '{account.Role}', '{account.Salt}')";
                    await command.ExecuteNonQueryAsync();
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

    /// <summary>
    /// Retrieves a user account by username
    /// </summary>
    /// <param name="username">Username to search for</param>
    /// <returns>UserModel if found, null if not found</returns>
    public async Task<UserModel> SelectAccountByUsername(string username)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM Accounts WHERE Username = '{username}'";
                    var result = await command.ExecuteReaderAsync();
                    if (!result.HasRows)
                    {
                        Console.WriteLine("Account does not exist.");
                        return null;
                    }
                    UserModel account = new UserModel();
                    while (await result.ReadAsync())
                    {
                        account.Id = result.GetInt32(0); 
                        account.Username = result.GetString(1); 
                        account.LastName = result.GetString(2);
                        account.FirstName = result.GetString(3);
                        account.Email = result.GetString(4);
                        account.PasswordHash = result.GetString(5);
                        account.Role = result.GetString(6);
                        account.Salt = result.GetString(7);
                    }
                    return account;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error selecting account by username");
            Console.WriteLine(e);
            throw;
        }
    }
    
    /// <summary>
    /// Retrieves a user account by email
    /// </summary>
    /// <param name="email">Email address to search for</param>
    /// <returns>UserModel if found, null if not found</returns>
    public async Task<UserModel> SelectAccountByEmail(string email)
    {
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM Accounts WHERE Email = '{email}'";
                    var result = await command.ExecuteReaderAsync();
                    if (!result.HasRows)
                    {
                        Console.WriteLine("Account does not exist.");
                        return null;
                    }
                    UserModel account = new UserModel();
                     while (await result.ReadAsync())
                    {
                        account.Id = result.GetInt32(0); 
                        account.Username = result.GetString(1); 
                        account.LastName = result.GetString(2);
                        account.FirstName = result.GetString(3);
                        account.Email = result.GetString(4);
                        account.PasswordHash = result.GetString(5);
                        account.Role = result.GetString(6);
                        account.Salt = result.GetString(7);
                    }
                    return account;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error selecting account by email");
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Retrieves all user accounts from the database
    /// </summary>
    /// <returns>List of UserModel objects, empty list if no accounts exist</returns>
    public async Task<List<UserModel>> GetAllAccounts()
    {
        List<UserModel> accounts = new List<UserModel>();
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Accounts";
                    var result = await command.ExecuteReaderAsync();
                
                    if (!result.HasRows)
                    {
                        Console.WriteLine("No accounts found.");
                        return accounts;
                    }

                    while (await result.ReadAsync())
                    {
                        UserModel account = new UserModel();
                        account.Id = result.GetInt32(0);
                        account.Username = result.GetString(1);
                        account.LastName = result.GetString(2);
                        account.FirstName = result.GetString(3);
                        account.Email = result.GetString(4);
                        account.PasswordHash = result.GetString(5);
                        account.Role = result.GetString(6);
                        accounts.Add(account);
                    }
                    return accounts;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error selecting all accounts");
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Creates a new user with hashed password
    /// </summary>
    /// <param name="userModel">User data to insert</param>
    /// <returns>True if successful, false if failed</returns>
    public async Task<bool> InsertUser(UserModel userModel)
    {
        userModel.PasswordHash = HashHelper.HashGenerate(userModel.PasswordHash, userModel.Salt);
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText =
                        $"INSERT INTO Accounts (Firstname, Lastname, Username, Email, PasswordHash, Role, Salt) VALUES ('{userModel.FirstName}','{userModel.LastName}','{userModel.Username}', '{userModel.Email}', '{userModel.PasswordHash}', '{userModel.Role}', '{userModel.Salt}')";
                    await command.ExecuteNonQueryAsync();
                    
                    command.CommandText = $"SELECT * FROM Accounts WHERE Email = '{userModel.Email}'";
                    var result = await command.ExecuteReaderAsync();
                    if (!result.HasRows)
                    {
                        Console.WriteLine("Error inserting account");
                        return false;
                    }
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
    
    /// <summary>
    /// Updates an existing user account
    /// </summary>
    /// <param name="toUpdate">Updated user data</param>
    /// <param name="currentUser">User initiating the update</param>
    /// <returns>True if update succeeded, false if unauthorized</returns>
    public async Task<bool> UpdateAccount(UserModel toUpdate, UserModel currentUser)
    {
        bool isAdmin = await IsAdmin(currentUser.Email, currentUser.PasswordHash);
        bool isSameUser = currentUser.Email == toUpdate.Email;
        
        if (!isAdmin && !isSameUser)
        {
            Console.WriteLine("You are not an admin and you are not the user you are trying to update.");
            return false;
        }
     
        try
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = $"UPDATE Accounts SET Firstname = '{toUpdate.FirstName}', Lastname = '{toUpdate.LastName}', Username = '{toUpdate.Username}', Email = '{toUpdate.Email}', PasswordHash = '{toUpdate.PasswordHash}', Role = '{toUpdate.Role}' WHERE Email = '{currentUser.Email}'";
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error updating account");
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Deletes a user account
    /// </summary>
    /// <param name="toDelete">User to be deleted</param>
    /// <param name="currentUser">User initiating the deletion</param>
    /// <returns>True if deletion succeeded, false if unauthorized</returns>
    public async Task<bool> DeleteAccount(UserModel toDelete, UserModel currentUser)
    {
        bool isAdmin = await IsAdmin(currentUser.Email, currentUser.PasswordHash);
        bool isSameUser = currentUser.Email == toDelete.Email;
        if (isAdmin || isSameUser)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM Accounts WHERE Email = '{toDelete.Email}'";
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error deleting account");
                Console.WriteLine(e);
                throw;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Checks if a user has admin privileges
    /// </summary>
    /// <param name="email">User's email</param>
    /// <param name="password">Plaintext password</param>
    /// <returns>True if user is an admin, false otherwise</returns>
    public async Task<bool> IsAdmin(string email, string password)
    {
        UserModel userModel = await SelectAccountByEmail(email);
        bool isPasswordCorrect = HashHelper.ValliedatePassword(password, userModel.Salt, userModel.PasswordHash);
        if (!isPasswordCorrect)
        {
            Console.WriteLine("Password is not correct.");
            return false;
        }
        return userModel.Role == "Admin";
    }
}