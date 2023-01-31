using System.Data;
using ErrorOr;
using Microsoft.Identity.Client;
using TestDotnetAPI.Common.Utils;
using TestDotnetAPI.ServiceErrors;

namespace TestDotnetAPI.Services.Authentication;
using TestDotnetAPI.Common.Authentication;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Users;
using TestDotnetAPI.Services.Database;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserService _userService;

    public AuthenticationService(IUserService userService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userService = userService;
    }

    public ErrorOr<AuthServiceResponse> Login(string userName, string password)
    {
        //check for existed username and password
        string sql1 = $"SELECT * FROM {DatabaseService.ACCOUNT_TABLE} WHERE UserName = '{userName}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql1);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        if (table.Rows.Count == 0) return Errors.Authentication.InvalidUsernameOrPassword;

        var userResult = User.From(table.AsEnumerable().First());
        if (userResult.IsError) return userResult.Errors;
        var user = userResult.Value;

        //get salt and check password
        if (!Util.VerifyPassword(password, user.Password, user.Salt))
        {
            // System.Console.WriteLine(Convert.ToHexString(user.Salt));
            return Errors.Authentication.InvalidUsernameOrPassword;
        }

        //generate token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role);
        return new AuthServiceResponse(user, token);

    }
    public ErrorOr<AuthServiceResponse> Register(User user)
    {
        ErrorOr<Created> result = _userService.CreateUser(user);
        if (result.IsError) return result.Errors;
        //generate token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role);
        return new AuthServiceResponse(user, token);
    }
}