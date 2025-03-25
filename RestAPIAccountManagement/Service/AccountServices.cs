using RestAPIAccountManagement.DAL;
using RestAPIAccountManagement.Hashing;
using RestAPIAccountManagement.Models.UserModel;
using RestAPIAccountManagement.Mapper;

namespace RestAPIAccountManagement.Service;

public  class AccountServices
{
    private readonly AccountDAL _accountDAL = new AccountDAL();
    private readonly MapperService _mapperService = new MapperService();
    private readonly HashHelper _hashHelper = new HashHelper();
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

    public async Task<bool> LoginService(string emailOrName, string password)
    {
        int check = CheckIsEmailOrUsername(emailOrName);
        return await SelectEmailLoginOrUsernameLogin(emailOrName, password, check);
    }

    public int CheckIsEmailOrUsername(string emailOrUsername)
    {
        if (emailOrUsername.Contains("@"))
        {
            return 1;
        }
        
        return 0;
    }
    public async Task<bool> SelectEmailLoginOrUsernameLogin(string emailOrUsername,string password, int check)
    {
       if(check == 1)return await _accountDAL.SelectAccountByEmailAndPassword(emailOrUsername, password);
        return await _accountDAL.SelectAccountByUsernameAndPassword(emailOrUsername, password);
    }
    

    public async Task<bool> RegisterService(UserModel userModel)
    {
        if (userModel == null) return false;
        userModel.Salt = _hashHelper.SaltGenerate();
        return  await _accountDAL.InsertUser(userModel);
        
    }
    public async Task<bool> UpdateUserService(UserModel userModel, UserModel currentUser)
    {
        if (userModel == null) return false;
        return await _accountDAL.UpdateAccount(userModel, currentUser);
    }
    public async Task<bool> DeleteUserService(UserModel userModel, UserModel currentUser)
    {
        if (userModel == null) return false;
        return await _accountDAL.DeleteAccount(userModel, currentUser);
    }

    public async Task<List<DTOGetAllAccounts>> GetAllAccountsService()
    {
        List<UserModel> allUsers = await _accountDAL.GetAllAccounts();
        if (allUsers == null)
        {
            Console.Error.WriteLine("Error getting all accounts.");
            return null;
        }
        List<DTOGetAllAccounts> allUsersDTO = new List<DTOGetAllAccounts>();
        foreach (var VARIABLE in allUsers)
        {
          DTOGetAllAccounts allAccounts =_mapperService.MapperGetAllAccountsDTO(VARIABLE);
          allUsersDTO.Add(allAccounts);
        }
        return  allUsersDTO;
    }
}