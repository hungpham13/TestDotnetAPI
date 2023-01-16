using TestDotnetAPI.Models;
using TestDotnetAPI.ServiceErrors;
using ErrorOr;
using TestDotnetAPI.Services.Database;

namespace TestDotnetAPI.Services.Users;

public class UserService : IUserService
{
    private static readonly Dictionary<Guid, User> _users = new();

    public ErrorOr<Created> CreateUser(User user)
    {
        _users.Add(user.Id, user);
        try
        {
            DatabaseService.update("INSERT INTO [dbo].[Account] ([Id], [Name], [Role], [PhoneNumber], [Password], [Active], [ActiveTimeStart], [ActiveTimeEnd]) VALUES" +
                $"('{user.Id}', '{user.Name}', '{user.Role}', '{user.PhoneNumber}', '{user.Password}', {(user.Active ? 1 : 0)}, {user.ActiveTimeStart}, {user.ActiveTimeEnd});", 'i');
            return Result.Created;
        }
        catch (Exception e)
        {
            return Errors.User.DBError(e.ToString());
        }
    }

    public ErrorOr<Deleted> DeleteUser(Guid id)
    {
        _users.Remove(id);
        return Result.Deleted;
    }

    public ErrorOr<User> GetUser(Guid id)
    {
        if (_users.ContainsKey(id))
        {
            return _users[id];
        }
        return Errors.User.NotFound;
    }

    public ErrorOr<UpdatedUser> UpdateUser(User user)
    {
        var isNewlyCreated = !_users.ContainsKey(user.Id);
        _users[user.Id] = user;
        return new UpdatedUser(isNewlyCreated);
    }
}