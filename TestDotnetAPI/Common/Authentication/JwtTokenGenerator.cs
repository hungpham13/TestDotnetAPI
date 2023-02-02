using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TestDotnetAPI.Models;
using ErrorOr;
using TestDotnetAPI.ServiceErrors;

namespace TestDotnetAPI.Common.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private string key = "super-secret-key";
    public string GenerateToken(Guid userId, string userName, string role)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256
        );
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Typ, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var securityToken = new JwtSecurityToken(
            issuer: "VtcAPI",
            expires: DateTime.Now.AddMinutes(60),
            claims: claims,
            signingCredentials: signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public ErrorOr<string> ValidateToken(string token, User.VALID_ROLES[]? validRoles = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            var userRole = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Typ).Value;
            if (validRoles == null) return userId;
            if (Array.IndexOf(validRoles.Select(r => r.ToString()).ToArray(), userRole) == -1)
                return Errors.Authentication.UnauthorizedRole;

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            return Errors.Authentication.InvalidToken;
        }
    }
}