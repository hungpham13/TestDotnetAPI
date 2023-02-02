using ErrorOr;

namespace TestDotnetAPI.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $"Breakfast name must be between {Models.Breakfast.MIN_NAME_LENGTH} and {Models.Breakfast.MAX_NAME_LENGTH} characters long");
        public static Error InvalidDescription => Error.Validation(
            code: "Breakfast.InvalidDescription",
            description: $"Breakfast description must be between {Models.Breakfast.MIN_NAME_LENGTH} and {Models.Breakfast.MAX_NAME_LENGTH} characters long");
        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast not found");
    }
    public static class User
    {
        public static Error UsernameExisted => Error.Validation(
            code: "User.UsernameExisted",
            description: "Username already existed");
        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User not found");
        public static Error InvalidName => Error.Validation(
            code: "User.InvalidName",
            description: $"Name must be not longer than {Models.User.MAX_NAME_LENGTH} characters");
        public static Error InvalidUserName => Error.Validation(
            code: "User.InvalidUserName",
            description: $"Username must be not longer than {Models.User.MAX_USERNAME_LENGTH} characters");
        public static Error InvalidPassword => Error.Validation(
            code: "User.InvalidPassword",
            description: $"Password must be between {Models.User.MIN_PASSWORD_LENGTH} and {Models.User.MAX_PASSWORD_LENGTH} characters long");
        public static Error InvalidPhoneNumber => Error.Validation(
            code: "User.InvalidPhoneNumber",
            description: $"Phone number must be not longer than {Models.User.MAX_PHONENUMBER_LENGTH} characters");
        public static Error InvalidRole => Error.Validation(
            code: "User.InvalidRole",
            description: $"Role must be {string.Join(", ", Enum.GetNames(typeof(Models.User.VALID_ROLES))[..^1])} or {Enum.GetNames(typeof(Models.User.VALID_ROLES))[^1]}");
        public static Error InvalidActiveTime => Error.Validation(
            code: "User.InvalidActiveTime",
            description: $"Start active time must be before end active time");
    }

    public static class Event
    {
        public static Error InvalidStatus => Error.Validation(
            code: "Event.InvalidStatus",
            description: $"Status must be {string.Join(", ", Enum.GetNames(typeof(Models.Event.VALID_STATUS))[..^1])} or {Enum.GetNames(typeof(Models.Event.VALID_STATUS))[^1]}");
        public static Error InvalidName => Error.Validation(
            code: "Event.InvalidName",
            description: $"Event name must not longer than {Models.Event.MAX_NAME_LENGTH} characters long");
        public static Error InvalidPerformerName => Error.Validation(
            code: "Event.InvalidPerformerName",
            description: $"Performer name must not longer than {Models.Event.MAX_PERFORMERNAME_LENGTH} characters long");
        public static Error InvalidUrl => Error.Validation(
            code: "Event.InvalidUrl",
            description: $"Image Url must not longer than {Models.Event.MAX_MAINIMAGE_LENGTH} characters long");
        public static Error InvalidDescription => Error.Validation(
            code: "Event.InvalidDescription",
            description: $"Description must not longer than {Models.Event.MAX_DESCRIPTION_LENGTH} characters long");
        public static Error NotFound => Error.NotFound(
            code: "Event.NotFound",
            description: "Event not found");
    }
    public static class Stream
    {
        public static Error InvalidUrl => Error.Validation(
            code: "Stream.InvalidUrl",
            description: $"Stream url must not longer than {Models.Stream.MAX_URL_LENGTH} characters long");
        public static Error InvalidName => Error.Validation(
            code: "Stream.InvalidName",
            description: $"Stream name must not longer than {Models.Stream.MAX_NAME_LENGTH} characters long");
    }
    public static class Attendance
    {
        public static Error InvalidStatus => Error.Validation(
            code: "Attendance.InvalidStatus",
            description: $"Status must be {string.Join(", ", Enum.GetNames(typeof(Models.Attendance.VALID_STATUS))[..^1])} or {Enum.GetNames(typeof(Models.Attendance.VALID_STATUS))[^1]}");
        public static Error NotFound => Error.NotFound(
            code: "Attendance.NotFound",
            description: "Attendance not found");
    }
    public static class Authentication
    {
        public static Error InvalidUsernameOrPassword => Error.Validation(
            code: "Authentication.InvalidUsernameOrPassword",
            description: "Invalid username or password.");

        public static Error UnauthorizedRole => Error.Validation(
            code: "Authentication.UnauthorizedRole",
            description: "Permission denied!");
        public static Error InvalidToken => Error.Validation(
            code: "Authentication.InvalidToken",
            description: "Invalid token.");
        
    }
    public static class Database
    {
        public static Error QueryError(string e) => Error.Failure(
            code: "Database.QueryError",
            description: e);
        public static Error UpdateError(string e) => Error.Failure(
            code: "Database.UpdateError",
            description: e);
    }
}
