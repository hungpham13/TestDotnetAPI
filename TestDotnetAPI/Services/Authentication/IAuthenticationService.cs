using TestDotnetAPI.Models;
using ErrorOr;

namespace TestDotnetAPI.Services.Authentication;

public interface IAuthenticationService
{
    ErrorOr<AuthServiceResponse> Login(string userName, string password);
    ErrorOr<AuthServiceResponse> Register(User user);
}