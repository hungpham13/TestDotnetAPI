namespace TestDotnetAPI.Services.Authentication;
using TestDotnetAPI.Common.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    ErrorOr<string> Login(
}