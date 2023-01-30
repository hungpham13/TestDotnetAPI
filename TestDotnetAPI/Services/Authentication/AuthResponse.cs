using TestDotnetAPI.Models;
namespace TestDotnetAPI.Services.Authentication;

public record AuthResponse(
    User User,
    string Token
);