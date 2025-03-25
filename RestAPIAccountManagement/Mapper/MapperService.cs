using RestAPIAccountManagement.Models.UserModel;

namespace RestAPIAccountManagement.Mapper;

public class MapperService
{
    /// <summary>
    ///   Maps a UserModel to a DTOGetAllAccounts object.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public  DTOGetAllAccounts MapperGetAllAccountsDTO(UserModel user)
    {
        var dtoGetAllAccounts = new DTOGetAllAccounts()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        };

        return dtoGetAllAccounts;
    }


}