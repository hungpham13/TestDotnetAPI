namespace TestDotnetAPI.Contracts.Authentication
{
    public record ChangePasswordRequest(
        string UserId,
        string OldPassword,
        string NewPassword
    );
}