namespace TestDotnetAPI.Common.Authentication;
public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string userName, string role);
}