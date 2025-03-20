using Microsoft.AspNetCore.Mvc;
using RestAPIAccountManagement.DAL;

namespace RestAPIAccountManagement.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    [Route("Account/login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }
}