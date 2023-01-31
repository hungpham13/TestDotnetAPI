using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.Authentication;
using TestDotnetAPI.Services.Authentication;
using ErrorOr;

namespace TestDotnetAPI.Controllers;

public class AuthController : ApiController
{
    private readonly IAuthenticationService _authenticationService;
    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        ErrorOr<AuthServiceResponse> loginResult = _authenticationService.Login(request.UserName, request.Password);
        return loginResult.Match(
            response => Ok(new AuthResponse(UsersController.MapUserResponse(response.User), response.Token)),
            errors => Problem(errors)
        );
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr<Models.User> requestToUserFormat = Models.User.From(request);
        if (requestToUserFormat.IsError)
        {
            return Problem(requestToUserFormat.Errors);
        }
        var user = requestToUserFormat.Value;
        ErrorOr<AuthServiceResponse> loginResult = _authenticationService.Register(user);
        return loginResult.Match(
            response => Ok(new AuthResponse(UsersController.MapUserResponse(user), response.Token)),
            errors => Problem(errors)
        );
    }
}