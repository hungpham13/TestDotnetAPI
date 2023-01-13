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

    }
}
