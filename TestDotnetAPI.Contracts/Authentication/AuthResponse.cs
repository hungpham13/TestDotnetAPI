using TestDotnetAPI.Contracts.User;

namespace TestDotnetAPI.Contracts.Authentication;

public record AuthResponse(
    UserResponse User,
    string Token
);