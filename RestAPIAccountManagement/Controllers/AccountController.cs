using Microsoft.AspNetCore.Mvc;

namespace RestAPIAccountManagement.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    [Route("Account/login")]
    public IActionResult Login()
    {
        return Ok();
    }
}