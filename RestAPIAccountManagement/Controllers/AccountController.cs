using Microsoft.AspNetCore.Mvc;
using RestAPIAccountManagement.DAL;
using RestAPIAccountManagement.Models.UserModel;
using RestAPIAccountManagement.Service;

namespace RestAPIAccountManagement.Controllers;

public class AccountController : Controller
{
    private readonly AccountServices _accountServices = new AccountServices();
    
    /// <summary>
    /// Authenticates a user by email/username and password
    /// </summary>
    /// <param name="emailOrName">User's email address or username</param>
    /// <param name="password">User's password (plaintext)</param>
    /// <returns>
    /// HTTP 200 OK if authentication succeeds,
    /// HTTP 400 Bad Request if parameters are missing,
    /// HTTP 401 Unauthorized if credentials are invalid
    /// </returns>
    [HttpGet]
    [Route("Account/login")]
    public async Task<IActionResult> Login([FromQuery] string emailOrName, [FromQuery] string password)
    {
        if (string.IsNullOrEmpty(emailOrName) || string.IsNullOrEmpty(password))
        {
            return BadRequest();
        }
        if (await _accountServices.LoginService(emailOrName, password))
        {
            return Ok();
        }
        return Unauthorized();
    }

    /// <summary>
    /// Registers a new user account
    /// </summary>
    /// <param name="userModel">User data transfer object containing registration details</param>
    /// <returns>
    /// HTTP 200 OK if registration succeeds,
    /// HTTP 400 Bad Request if user data is invalid or registration fails
    /// </returns>
    /// <remarks>
    /// Sample request:
    /// {
    ///     "firstName": "Max",
    ///     "lastName": "Mustermann",
    ///     "username": "maxmustermann",
    ///     "email": "max@example.com",
    ///     "passwordHash": "SecurePass123!",
    ///     "role": "User"
    /// }
    /// </remarks>
    [HttpPost]
    [Route("Account/Register")]
    public async Task<IActionResult> Register([FromBody] UserModel userModel)
    {
        if (userModel == null)
        {
            return BadRequest();
        }
        if (await _accountServices.RegisterService(userModel))
        {
            return Ok();
        }
        return BadRequest();
    }

    /// <summary>
    /// Updates an existing user account
    /// </summary>
    /// <param name="dtoUpdateUserModel">
    /// Data transfer object containing:
    /// - ToUpdate: User data to update
    /// - CurrentUser: Authenticated user making the request
    /// </param>
    /// <returns>
    /// HTTP 200 OK if update succeeds,
    /// HTTP 400 Bad Request if data is invalid or update fails
    /// </returns>
    /// <response code="403">Returned if current user lacks permission to update</response>
    [HttpPut] 
    [Route("Account/Update")]
    public async Task<IActionResult> Update([FromBody] DTOUpdate dtoUpdateUserModel)
    {
        if (dtoUpdateUserModel == null)
        {
            return BadRequest();
        }
        if (await _accountServices.UpdateUserService(dtoUpdateUserModel.ToUpdate, dtoUpdateUserModel.CurrentUser))
        {
            return Ok();
        }
        return BadRequest();
    }

    /// <summary>
    /// Deletes a user account
    /// </summary>
    /// <param name="dtoDeleteUserModel">
    /// Data transfer object containing:
    /// - User: User to be deleted
    /// - CurrentUser: Authenticated user making the request
    /// </param>
    /// <returns>
    /// HTTP 200 OK if deletion succeeds,
    /// HTTP 400 Bad Request if deletion fails,
    /// HTTP 403 Forbidden if unauthorized attempt
    /// </returns>
    [HttpDelete]
    [Route("Account/Delete")]
    public async Task<IActionResult> Delete([FromBody] DTODelete dtoDeleteUserModel)
    {
        bool isDeleted = await _accountServices.DeleteUserService(dtoDeleteUserModel.User, dtoDeleteUserModel.CurrentUser);
        if (isDeleted)
        {
            Console.WriteLine("User deleted");
            return Ok();
        }
        Console.WriteLine("User not found");
        return BadRequest();
    }

    /// <summary>
    /// Retrieves all user accounts
    /// </summary>
    /// <returns>
    /// HTTP 200 OK with list of all users,
    /// HTTP 500 Internal Server Error if retrieval fails
    /// </returns>
    [HttpGet]
    [Route("Account/GetAll")]
    public async Task<IActionResult> GetAllUser()
    {
        return Ok(await _accountServices.GetAllAccountsService());
    }
}