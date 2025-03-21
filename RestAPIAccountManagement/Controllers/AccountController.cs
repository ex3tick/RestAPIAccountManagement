using Microsoft.AspNetCore.Mvc;
using RestAPIAccountManagement.DAL;
using RestAPIAccountManagement.Models.UserModel;
using RestAPIAccountManagement.Service;

namespace RestAPIAccountManagement.Controllers;

public class AccountController : Controller
{
    private readonly AccountServices _accountServices = new AccountServices();
    [HttpGet]
    [Route("Account/login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }
    [HttpGet]
    [Route("Account/Register")]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}