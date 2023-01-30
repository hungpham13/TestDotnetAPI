namespace TestDotnetAPI.Services.Authentication;
using TestDotnetAPI.Common.Authentication;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Users;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserService _userService;

    ErrorOr<AuthResponse> Login(string userName, string password)
    {
        //check for existed username and password
        string sql1 = $"SELECT * FROM {DatabaseService.ACCOUNT_TABLE} WHERE UserName = '{user.UserName}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql1);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        if (table.Rows.Count == 0) return Errors.Authentication.InvalidUsernameOrPassword;

        var user = User.From(table.AsEnumerable().First());

        //get salt and check password
        if (!Util.VerifyPassword(password, user.Password, user.Salt))
        {
            return Errors.Authentication.InvalidUsernameOrPassword;
        }

        //generate token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role);
        return new AuthResponse(user, token);

    }
    ErrorOr<AuthResponse> Register(User user)
    {
        ErrorOr<Created> result = _userService.CreateUser(user);
        //generate token
        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.UserName, user.Role);
        return new AuthResponse(user, token);
    }
}