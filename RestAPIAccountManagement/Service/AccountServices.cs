using RestAPIAccountManagement.DAL;
using RestAPIAccountManagement.Models.UserModel;

namespace RestAPIAccountManagement.Service;

public  class AccountServices
{
    private readonly AccountDAL _accountDAL = new AccountDAL();
    public async Task<bool> InserUserService(UserModel userModel)
    {
        if (userModel == null)
        {
            return false;
        }
        if(await _accountDAL.InsertUser(userModel))
        {
            Console.WriteLine("User inserted.");
            return true;
        }
        Console.Error.WriteLine("Error inserting user.");
        return false;
    }
}