using System.Security.Cryptography;
using System.Text;
using RestAPIAccountManagement.Models.HashModel;

namespace RestAPIAccountManagement.Hashing;

public class HashHelper
{
    /// <summary>
    ///  This method is used to hash the password with the salt and pepper
    /// </summary>
    /// <param name="Password"></param>
    /// <param name="Salt"></param>
    /// <param name="Pepper"></param>
    /// <returns></returns>
    public static string HashGenerate(string Password, string Salt, string Pepper)
    {
        string combinedstring = Password + Salt;

        byte[] combinedBytes = Encoding.UTF8.GetBytes(combinedstring);

        using (var sha256 = new HMACSHA256(Encoding.UTF8.GetBytes(Pepper)))
        {
            byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

            string base64Hash = System.Convert.ToBase64String(hashedBytes);

            return base64Hash;
        }
        
    }
    
    public static string SaltGenerate()
    {
        var rng = RandomNumberGenerator.Create(); 
        byte[] saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        return Convert.ToBase64String(saltBytes);
    }
    
    
   
    public static HashSaltModel HashWithSalt(string password)
    {
    
        HashSaltModel model = new HashSaltModel();
        model.Salt = SaltGenerate();
        model.Password = HashGenerate(password, model.Salt, "Pepper");
        return model;
    }
 
    
    public static bool ValliedatePassword(string password,  string salt, string hashPassword)
    {
        string hash = HashGenerate(password, salt, "Pepper");
        return hash == hashPassword;
       
    }
}