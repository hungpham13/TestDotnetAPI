using TestDotnetAPI.Models;
using ErrorOr;

namespace TestDotnetAPI.Common.Authentication;
public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string userName, string role);
    ErrorOr<string> ValidateToken(string token, User.VALID_ROLES[]? validRoles = null);
}