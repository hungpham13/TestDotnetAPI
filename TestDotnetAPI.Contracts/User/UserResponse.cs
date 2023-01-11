namespace TestDotnetAPI.Contracts.User;

public record UserResponse(
    Guid Id,
    string Name,
    string UserName,
    string PhoneNumber,
    string Role,
    bool Active,
    DateTime ActiveTimeStart,
    DateTime ActiveTimeEnd
);