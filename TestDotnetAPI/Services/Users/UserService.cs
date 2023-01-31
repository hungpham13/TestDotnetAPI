using TestDotnetAPI.Models;
using TestDotnetAPI.ServiceErrors;
using ErrorOr;
using TestDotnetAPI.Services.Database;
using Microsoft.Data.SqlClient;
using System.Data;
using TestDotnetAPI.Common.Utils;

namespace TestDotnetAPI.Services.Users;

public class UserService : IUserService
{
    // private static readonly Dictionary<Guid, User> _users = new();

    public ErrorOr<Created> CreateUser(User user)
    {
        // _users.Add(user.Id, user);
        //check for existed username
        string sql1 = $"SELECT * FROM {DatabaseService.ACCOUNT_TABLE} WHERE UserName = '{user.UserName}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql1);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        if (table.Rows.Count > 0) return Errors.User.UsernameExisted;

        //hash password
        var hash = Util.HashPasword(user.Password, out byte[] salt);

        string sql;
        if (user.ActiveTimeStart == DateTime.MinValue)
        {
            sql =
                $"INSERT INTO {DatabaseService.ACCOUNT_TABLE} ([Id], [UserName], [Name], [Role], [PhoneNumber], [Password], [Active], [Salt]) VALUES" +
                $"('{user.Id}', '{user.UserName}', '{user.Name}', '{user.Role}', '{user.PhoneNumber}', '{hash}', {(user.Active ? 1 : 0)}, '{Convert.ToHexString(salt)}');";
        }
        else
        {
            sql =
                $"INSERT INTO {DatabaseService.ACCOUNT_TABLE} ([Id], [UserName], [Name], [Role], [PhoneNumber], [Password], [Active], [ActiveTimeStart], [ActiveTimeEnd], [Salt]) VALUES" +
                $"('{user.Id}', '{user.UserName}', '{user.Name}', '{user.Role}', '{user.PhoneNumber}', '{hash}', {(user.Active ? 1 : 0)}, '{user.ActiveTimeStart}', '{user.ActiveTimeEnd}', '{Convert.ToHexString(salt)}');";
        }
        return DatabaseService.insert(sql);
    }

    public ErrorOr<Deleted> DeleteUser(Guid id)
    {
        // _users.Remove(id);
        string sql = $"DELETE FROM {DatabaseService.ACCOUNT_TABLE} WHERE Id = '{id}';";
        return DatabaseService.delete(sql);
    }

    public ErrorOr<User> GetUser(Guid id)
    {
        string sql = $"SELECT * FROM {DatabaseService.ACCOUNT_TABLE} WHERE Id = '{id}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        if (table.Rows.Count > 0)
        {
            return User.From(table.AsEnumerable().First());
        }
        return Errors.User.NotFound;
    }

    public ErrorOr<Updated> UpdateUser(User user)
    {
        //check for existed username
        string sql1 = $"SELECT * FROM {DatabaseService.ACCOUNT_TABLE} WHERE UserName = '{user.UserName}' AND Id != '{user.Id}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql1);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        if (table.Rows.Count > 0) return Errors.User.UsernameExisted;

        string sql = $"UPDATE {DatabaseService.ACCOUNT_TABLE} SET " +
            $"[UserName] = '{user.UserName}', " +
            $"[Name] = '{user.Name}', " +
            $"[Role] = '{user.Role}', " +
            $"[PhoneNumber] = '{user.PhoneNumber}', " +
            $"[Password] = '{user.Password}', " +
            $"[Active] = {(user.Active ? 1 : 0)}, " +
            $"[ActiveTimeStart] = '{user.ActiveTimeStart}', " +
            $"[ActiveTimeEnd] = '{user.ActiveTimeEnd}' " +
            $"WHERE Id = '{user.Id}';";
        // _users[user.Id] = user;
        return DatabaseService.update(sql);
    }
    public ErrorOr<List<User>> GetUsers(int page, int size)
    {
        List<User> users = new();
        string sql = "SELECT * FROM (" +
            "SELECT ROW_NUMBER() OVER (ORDER BY [ActiveTimeStart] DESC) AS rownumber, * " +
            $"FROM {DatabaseService.ACCOUNT_TABLE}) AS foo " +
            $"WHERE rownumber <= {size * page} and rownumber > {size * (page - 1)};";
        ErrorOr<DataTable> result = DatabaseService.query(sql);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        foreach (DataRow row in table.Rows)
        {
            var user = User.From(row);
            if (user.IsError) return user.Errors;
            users.Add(user.Value);
        }
        return users;
    }
}