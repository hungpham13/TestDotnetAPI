namespace TestDotnetAPI.Contracts.Authentication;

public record RegisterRequest(
    string UserName,
    string Name,
    string Role,
    string Password,
    string PhoneNumber
);