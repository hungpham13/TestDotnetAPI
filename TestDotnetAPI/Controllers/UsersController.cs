using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.User;
using ErrorOr;
using TestDotnetAPI.Models;

namespace TestDotnetAPI.Controllers;

public class UsersController : ApiController
{
    public UsersController()
    {
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        // transfer to model format
        ErrorOr<Breakfast> requestToBreakfastFormat = Breakfast.From(request);
        if (requestToBreakfastFormat.IsError)
        {
            return Problem(requestToBreakfastFormat.Errors);
        }
        var breakfast = requestToBreakfastFormat.Value;

        // save breakfast to database
        ErrorOr<Created> result = .CreateBreakfast(breakfast);

        // transfer to response format
        return result.Match(
            created => CreatedAsGetBreakfast(breakfast),
            errors => Problem(errors));
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

    private static UserResponse MapUserResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.UserName,
            user.PhoneNumber,
            user.Role,
            user.Active,
            user.ActiveTimeStart,
            user.ActiveTimeEnd
        );
    }

    private IActionResult CreatedAsGetUser(User user)
    {
        return CreatedAtAction(
                actionName: nameof(GetUser),
                routeValues: new { id = user.Id },
                value: MapUserResponse(user));
    }
}