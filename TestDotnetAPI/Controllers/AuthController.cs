using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.Authentication;

namespace TestDotnetAPI.Controllers;

public class AuthController : ApiController
{
    public AuthController()
    {
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        return Ok("Hello World");
    }

    [HttpPut("register")]
    public IActionResult Register(RegisterRequest request)
    {
        return Ok("Hello World");
    }
}