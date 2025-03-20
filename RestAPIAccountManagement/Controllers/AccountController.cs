using Microsoft.AspNetCore.Mvc;

namespace RestAPIAccountManagement.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    [Route("Account/login")]
    public async Task<IActionResult> Login()
    {string test = "test";
        return Ok(test);
    }
}