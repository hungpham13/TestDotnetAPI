using TestDotnetAPI.Models;
using ErrorOr;

namespace TestDotnetAPI.Services.Users;

public interface IUserService
{
    ErrorOr<Created> CreateUser(User user);
    ErrorOr<User> GetUser(Guid id);
    ErrorOr<UpdatedUser> UpdateUser(User user);
    ErrorOr<Deleted> DeleteUser(Guid id);
}