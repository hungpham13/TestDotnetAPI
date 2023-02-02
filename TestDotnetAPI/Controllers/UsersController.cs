using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.User;
using ErrorOr;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Users;
using TestDotnetAPI.Services.Database;
using System.Data;
using TestDotnetAPI.Common.Authentication;

namespace TestDotnetAPI.Controllers;

public class UsersController : ApiController
{

    private readonly IUserService _userService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    public UsersController(IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost]
    public IActionResult CreateUser(CreateUserRequest request, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, new []{ Models.User.VALID_ROLES.Admin });
        if (caller.IsError) return Problem(caller.Errors);
        
        // transfer to model format
        ErrorOr<Models.User> requestToUserFormat = Models.User.From(request);
        if (requestToUserFormat.IsError) return Problem(requestToUserFormat.Errors);
        var user = requestToUserFormat.Value;

        // save user to database
        ErrorOr<Created> result = _userService.CreateUser(user);

        // transfer to response format
        return result.Match(
            created => CreatedAsGetUser(user),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetUser(Guid id, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token);
        if (caller.IsError) return Problem(caller.Errors);
        ErrorOr<Models.User> userResult = _userService.GetUser(id);
        return userResult.Match(
            user => Ok(MapUserResponse(user)),
            errors => Problem(errors));
    }

    [HttpGet("")]
    public IActionResult GetUsers(int page, int size, string token)
    {
        // validate
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, new []{ Models.User.VALID_ROLES.Admin });
        if (caller.IsError) return Problem(caller.Errors);
        
        // get all
        string getTotalSql = $"SELECT COUNT(*) FROM {DatabaseService.ACCOUNT_TABLE}";
        ErrorOr<DataTable> result = DatabaseService.query(getTotalSql);
        if (result.IsError) return Problem(result.Errors);
        var table = result.Value;
        int total = table.AsEnumerable().First().Field<int>(0);

        ErrorOr<List<Models.User>> usersResult = _userService.GetUsers(page, size);
        if (usersResult.IsError) return Problem(usersResult.Errors);
        return Ok(new AllUserResponse(
            total,
            size == 0 ? 0 : (int)Math.Ceiling((double)total / size),
            page,
            usersResult.Value.Select(MapUserResponse).ToList()
        ));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteUser(Guid id, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, new []{ Models.User.VALID_ROLES.Admin });
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Deleted> result = _userService.DeleteUser(id);
        return result.Match(
            deleted => Ok(),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateUser(Guid id, string token, UpdateUserRequest request)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, new []{ Models.User.VALID_ROLES.Admin });
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Models.User> userResult = _userService.GetUser(id);
        if (userResult.IsError) return Problem(userResult.Errors);
        Models.User oldUser = userResult.Value;

        ErrorOr<Models.User> requestToUserFormat = Models.User.From(id, request, oldUser);
        if (requestToUserFormat.IsError)
        {
            return Problem(requestToUserFormat.Errors);
        }

        var user = requestToUserFormat.Value;

        ErrorOr<Updated> result = _userService.UpdateUser(user);

        return result.Match(
            updated => Ok(MapUserResponse(user)),
            errors => Problem(errors));
    }

    public static UserResponse MapUserResponse(User user)
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