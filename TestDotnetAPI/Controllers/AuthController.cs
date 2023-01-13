using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.Authentication;

namespace TestDotnetAPI.Controllers;

public class AuthController
{
    public AuthController()
    {
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        return Ok("Hello World");
    }

    [HttpPut("change-password")]
    public IActionResult ChangePassword(ChangePasswordRequest request)
    {
        return Ok("Hello World");
    }
}