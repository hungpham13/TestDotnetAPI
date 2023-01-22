namespace TestDotnetAPI.Contracts.User;

public record AllUserResponse(
    int TotalItems,
    int TotalPages,
    int CurrentPage,
    List<UserResponse> Users
);