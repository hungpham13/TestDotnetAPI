namespace TestDotnetAPI.Models;
using TestDotnetAPI.ServiceErrors;
using ErrorOr;
using TestDotnetAPI.Contracts.User;
public class User
{
    public Guid Id;
    public string UserName;
    public string Name;
    public string Password;
    public string Role;
    public string PhoneNumber;
    public bool Active;
    public DateTime ActiveTimeStart;
    public DateTime ActiveTimeEnd;

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
        // if (userName.Length is < MIN_USERNAME_LENGTH or > MAX_USERNAME_LENGTH)
        // {
        //     errors.Add(Errors.User.InvalidUserName);
        // }
        // if (name.Length is < MIN_NAME_LENGTH or > MAX_NAME_LENGTH)
        // {
        //     errors.Add(Errors.User.InvalidName);
        // }
        // if (password.Length is < MIN_PASSWORD_LENGTH or > MAX_PASSWORD_LENGTH)
        // {
        //     errors.Add(Errors.User.InvalidPassword);
        // }
        // if (role.Length is < MIN_ROLE_LENGTH or > MAX_ROLE_LENGTH)
        // {
        //     errors.Add(Errors.User.InvalidRole);
        // }
        // if (phoneNumber.Length is < MIN_PHONENUMBER_LENGTH or > MAX_PHONENUMBER_LENGTH)
        // {
        //     errors.Add(Errors.User.InvalidPhoneNumber);
        // }
        // if (activeTimeStart > activeTimeEnd)
        // {
        //     errors.Add(Errors.User.InvalidActiveTime);
        // }
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
}