using TestDotnetAPI.ServiceErrors;
using ErrorOr;
using TestDotnetAPI.Contracts.User;

namespace TestDotnetAPI.Models;
public class User
{
    public Guid Id { get; }
    public string UserName { get; }
    public string Name { get; }
    public string Password { get; }
    public string Role { get; }
    public string PhoneNumber { get; }
    public bool Active { get; }
    public DateTime ActiveTimeStart { get; }
    public DateTime ActiveTimeEnd { get; }
    public const int MAX_USERNAME_LENGTH = 50;
    public const int MAX_NAME_LENGTH = 50;
    public const int MAX_PASSWORD_LENGTH = 16;
    public const int MIN_PASSWORD_LENGTH = 8;
    public const int MAX_PHONENUMBER_LENGTH = 15;
    public enum VALID_ROLES { Admin, Manager, User };

    private User(
        Guid id,
        string userName,
        string name,
        string password,
        string role,
        string phoneNumber,
        bool active,
        DateTime activeTimeStart,
        DateTime activeTimeEnd
    )
    {
        Id = id;
        UserName = userName;
        Name = name;
        Password = password;
        Role = role;
        PhoneNumber = phoneNumber;
        Active = active;
        ActiveTimeStart = activeTimeStart;
        ActiveTimeEnd = activeTimeEnd;
    }
    private static ErrorOr<User> Create(
        string userName,
        string name,
        string password,
        string role,
        string phoneNumber,
        bool active,
        DateTime activeTimeStart,
        DateTime activeTimeEnd,
        Guid? id = null)
    {

        //inforce invariant
        List<Error> errors = new();
        if (userName.Length is > MAX_USERNAME_LENGTH)
        {
            errors.Add(Errors.User.InvalidUserName);
        }
        if (name.Length is > MAX_NAME_LENGTH)
        {
            errors.Add(Errors.User.InvalidName);
        }
        if (password.Length is < MIN_PASSWORD_LENGTH or > MAX_PASSWORD_LENGTH)
        {
            errors.Add(Errors.User.InvalidPassword);
        }
        if (Enum.IsDefined(typeof(VALID_ROLES), role) is false)
        {
            errors.Add(Errors.User.InvalidRole);
        }
        if (phoneNumber.Length is > MAX_PHONENUMBER_LENGTH)
        {
            errors.Add(Errors.User.InvalidPhoneNumber);
        }
        if (activeTimeStart > activeTimeEnd)
        {
            errors.Add(Errors.User.InvalidActiveTime);
        }
        if (errors.Count > 0)
        {
            return errors;
        }
        return new User(
            id ?? Guid.NewGuid(),
            userName,
            name,
            password,
            role,
            phoneNumber,
            active,
            activeTimeStart,
            activeTimeEnd
        );
    }
    public static ErrorOr<User> From(CreateUserRequest request)
    {
        return Create(
            request.UserName,
            request.Name,
            request.Password,
            request.Role,
            request.PhoneNumber,
            true,
            DateTime.Now,
            DateTime.Now.AddYears(1)
        );
    }
    public static ErrorOr<User> From(Guid id, UpdateUserRequest request)
    {
        return Create(
            request.UserName,
            request.Name,
            request.Password,
            request.Role,
            request.PhoneNumber,
            request.Active,
            request.ActiveTimeStart,
            request.ActiveTimeEnd,
            request.Id
        );
    }
}