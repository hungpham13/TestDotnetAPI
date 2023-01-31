using TestDotnetAPI.Models;
namespace TestDotnetAPI.Services.Authentication;

public record AuthServiceResponse(
    User User,
    string Token
);