using TestDotnetAPI.Models;
using ErrorOr;

namespace TestDotnetAPI.Services.Users;

public interface IUserService
{
    ErrorOr<Created> CreateUser(User user);
    ErrorOr<User> GetUser(Guid id);
    ErrorOr<Updated> UpdateUser(User user);
    ErrorOr<Deleted> DeleteUser(Guid id);
    ErrorOr<List<User>> GetUsers(int page, int size);
}