namespace TestDotnetAPI.Contracts.User;

public record UpdateUserRequest(
    string UserName,
    string Name,
    string Role,
    string PhoneNumber,
    bool Active,
    DateTime activeTimeStart,
    DateTime activeTimeEnd
);