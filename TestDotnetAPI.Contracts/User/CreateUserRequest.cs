namespace TestDotnetAPI.Contracts.User;

public record CreateUserRequest(
    string UserName,
    string Name,
    string Role,
    string Password,
    string PhoneNumber,
    bool Active,
    DateTime? ActiveTimeStart,
    DateTime? ActiveTimeEnd
);