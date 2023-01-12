using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.Users;

namespace TestDotnetAPI.Controllers;

public class UsersController : ApiController
{
    public UsersController()
    {
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        return Ok("Hello World");
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUser(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpGet("?page={page_num:int}&size={page_size:int}")]
    public IActionResult GetUsers(int page_num, int page_size)
    {
        return Ok("Hello World");
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateUser(Guid id, UpdateUserRequest request)
    {
        return Ok("Hello World");
    }
}