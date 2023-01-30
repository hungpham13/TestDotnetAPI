using TestDotnetAPI.ServiceErrors;
using ErrorOr;
using TestDotnetAPI.Contracts.User;
using TestDotnetAPI.Contracts.Authentication;
using Microsoft.Data.SqlClient;
using TestDotnetAPI.Common.Utils;

namespace TestDotnetAPI.Models;
public class User
{
    public Guid Id { get; }
    public string UserName { get; }
    public string Name { get; }
    public string Password { get; }
    public string Salt { get; }
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
    public static ErrorOr<User> From(RegisterRequest request)
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
    public static ErrorOr<User> From(CreateUserRequest request)
    {
        return Create(
            request.UserName,
            request.Name,
            request.Password,
            request.Role,
            request.PhoneNumber,
            request.Active,
            request.ActiveTimeStart ?? default,
            request.ActiveTimeEnd ?? default
        );
    }
    public static ErrorOr<User> From(Guid id, UpdateUserRequest request, User user)
    {
        return Create(
            request.UserName ?? user.UserName,
            request.Name ?? user.Name,
            user.Password,
            request.Role ?? user.Role,
            request.PhoneNumber ?? user.PhoneNumber,
            request.Active ?? user.Active,
            request.ActiveTimeStart ?? user.ActiveTimeStart,
            request.ActiveTimeEnd ?? user.ActiveTimeEnd,
            id
        );
    }
    public static ErrorOr<User> From(System.Data.DataRow reader)
    {
        return Create(
            reader["UserName"] as string,
            reader["Name"] as string,
            reader["Password"] as string,
            reader["Role"] as string,
            reader["PhoneNumber"] as string,
            (bool)reader["Active"],
            Util.ConvertFromDBVal<DateTime>(reader["ActiveTimeStart"]),
            Util.ConvertFromDBVal<DateTime>(reader["ActiveTimeEnd"]),
            Guid.Parse(reader["Id"].ToString())
        );
    }
}