using TestDotnetAPI.Contracts.Breakfast;
using TestDotnetAPI.ServiceErrors;
using ErrorOr;

namespace TestDotnetAPI.Models;

public class Breakfast
{
    public const int MIN_NAME_LENGTH = 3;
    public const int MAX_NAME_LENGTH = 50;

    public const int MIN_DESCRIPTION_LENGTH = 3;
    public const int MAX_DESCRIPTION_LENGTH = 50;

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }

    private Breakfast(
        Guid id,
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        DateTime lastModifiedDateTime,
        List<string> savory,
        List<string> sweet
    )
    {
        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        LastModifiedDateTime = lastModifiedDateTime;
        Savory = savory;
        Sweet = sweet;
    }
    private static ErrorOr<Breakfast> Create(
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        List<string> savory,
        List<string> sweet,
        Guid? id = null)
    {
        //inforce invariant
        List<Error> errors = new();
        if (name.Length is < MIN_NAME_LENGTH or > MAX_NAME_LENGTH)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }
        if (description.Length is < MIN_DESCRIPTION_LENGTH or > MAX_DESCRIPTION_LENGTH)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Breakfast(
            id ?? Guid.NewGuid(),
            name: name,
            description: description,
            startDateTime: startDateTime,
            endDateTime: endDateTime,
            lastModifiedDateTime: DateTime.UtcNow,
            savory: savory,
            sweet: sweet
        );
    }
    public static ErrorOr<Breakfast> From(CreateBreakfastRequest request)
    {
        return Create(
            name: request.Name,
            description: request.Description,
            startDateTime: request.StartDateTime,
            endDateTime: request.EndDateTime,
            savory: request.Savory,
            sweet: request.Sweet
        );
    }
    public static ErrorOr<Breakfast> From(Guid id, UpdateBreakfastRequest request)
    {
        return Create(
            name: request.Name,
            description: request.Description,
            startDateTime: request.StartDateTime,
            endDateTime: request.EndDateTime,
            savory: request.Savory,
            sweet: request.Sweet,
            id: id
        );
    }
}