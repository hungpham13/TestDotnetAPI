using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.User;
using ErrorOr;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Users;

namespace TestDotnetAPI.Controllers;

public class UsersController : ApiController
{

    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request)
    {
        // transfer to model format

        ErrorOr<Models.User> requestToUserFormat = Models.User.From(request);
        if (requestToUserFormat.IsError)
        {
            return Problem(requestToUserFormat.Errors);
        }
        var user = requestToUserFormat.Value;

        // save user to database
        ErrorOr<Created> result = _userService.CreateUser(user);

        // transfer to response format
        return result.Match(
            created => CreatedAsGetUser(user),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUser(Guid id)
    {
        ErrorOr<Models.User> userResult = _userService.GetUser(id);
        return userResult.Match(
            user => Ok(MapUserResponse(user)),
            errors => Problem(errors));
    }

    [HttpGet("")]
    public IActionResult GetUsers(int page_num, int page_size)
    {
        return Ok("Hello World");
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id)
    {
        ErrorOr<Deleted> result = _userService.DeleteUser(id);
        return result.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateUser(Guid id, UpdateUserRequest request)
    {
        ErrorOr<Models.User> requestToUserFormat = Models.User.From(id, request);
        if (requestToUserFormat.IsError)
        {
            return Problem(requestToUserFormat.Errors);
        }

        var user = requestToUserFormat.Value;

        ErrorOr<UpdatedUser> result = _userService.UpdateUser(user);

        return result.Match(
            updatedUser => updatedUser.isNewlyCreated
                ? CreatedAsGetUser(user)
                : Ok(MapUserResponse(user)),
            errors => Problem(errors));
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