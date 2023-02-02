using System.Data;
using ErrorOr;
using TestDotnetAPI.Contracts.Event;
using TestDotnetAPI.ServiceErrors;

namespace TestDotnetAPI.Models;

public class Event
{
    private Event(
        Guid id,
        string name,
        string description,
        string performerName,
        DateTime time,
        string status,
        List<Stream> streams,
        string mainImage,
        string coverImage
    )
    {
        Id = id;
        Name = name;
        Description = description;
        PerformerName = performerName;
        Time = time;
        Status = status;
        Streams = streams;
        MainImage = mainImage;
        CoverImage = coverImage;
    }

    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string PerformerName { get; }
    public DateTime Time { get; }
    public string Status { get; }
    public List<Stream>? Streams { get; }
    public string MainImage { get; }
    public string CoverImage { get; }

    public const int MAX_NAME_LENGTH = 50;
    public const int MAX_DESCRIPTION_LENGTH = 4000;
    public const int MAX_PERFORMERNAME_LENGTH = 50;
    public const int MAX_MAINIMAGE_LENGTH = 100;
    public const int MAX_COVERIMAGE_LENGTH = 100;

    public enum VALID_STATUS {
        Upcoming, Live, Past
    };
    

    public static ErrorOr<Event> Create(
        string name,
        string description,
        string performerName,
        DateTime time,
        string status,
        string mainImage,
        string coverImage,
        List<Stream>? streams = null,
        Guid? id = null
    )
    {
        List<Error> errors = new List<Error>();
        if (name.Length is > MAX_NAME_LENGTH)
            errors.Add(Errors.Event.InvalidName);
        if (performerName.Length is > MAX_PERFORMERNAME_LENGTH)
            errors.Add(Errors.Event.InvalidPerformerName);
        if (description.Length is > MAX_DESCRIPTION_LENGTH)
            errors.Add(Errors.Event.InvalidDescription);
        if (Enum.IsDefined(typeof(VALID_STATUS), status) is false)
            errors.Add(Errors.Event.InvalidStatus);
        if (mainImage.Length is > MAX_MAINIMAGE_LENGTH)
            errors.Add(Errors.Event.InvalidUrl);
        if (coverImage.Length is > MAX_COVERIMAGE_LENGTH)
            errors.Add(Errors.Event.InvalidUrl);
        if (errors.Count > 0)
            return errors;

        return new Event(
            id ?? Guid.NewGuid(),
            name,
            description,
            performerName,
            time,
            status,
            streams,
            mainImage,
            coverImage
        );
    }

    public static ErrorOr<Event> From(CreateEventRequest request)
    {
        var streams = Stream.From(request.Streams);
        if (streams.IsError) return streams.Errors;

        return Create(request.Name,
            request.Description,
            request.Performer,
            request.Time,
            request.Status,
            request.MainImage,
            request.CoverImage,
            streams.Value
        );
    }

    public static ErrorOr<Event> From(Guid id, UpdateEventRequest request, Event e)
    {
        List<Stream> streams; 
        if (request.Streams is not null)
        {
            var result = Stream.From(request.Streams);
            if (result.IsError) return result.Errors;
            streams = result.Value;
        }
        else
        {
            streams = e.Streams;
        }
        
        return Create(
            request.Name ?? e.Name,
            request.Description ?? e.Description,
            request.Performer ?? e.PerformerName,
            request.Time ?? e.Time,
            request.Status ?? e.Status,
            request.MainImage ?? e.MainImage,
            request.CoverImage ?? e.CoverImage,
            streams,
            id
        );
    }
    public static ErrorOr<Event> From(DataRow row, List<Stream> streams)
    {
        var id = (Guid)row["Id"];
        var name = (string)row["Name"];
        var description = (string)row["Description"];
        var performerName = (string)row["Performer"];
        var time = (DateTime)row["Time"];
        var status = (string)row["Status"];
        var mainImage = (string)row["MainImage"];
        var coverImage = (string)row["CoverImage"];

        return Create(
            name,
            description,
            performerName,
            time,
            status,
            mainImage,
            coverImage,
            streams,
            id
        );
    }

    public static ErrorOr<Event> From(DataRow row)
    {
        var id = (Guid)row["Id"];
        var name = (string)row["Name"];
        var description = (string)row["Description"];
        var performerName = (string)row["Performer"];
        var time = (DateTime)row["Time"];
        var status = (string)row["Status"];
        var mainImage = (string)row["MainImage"];
        var coverImage = (string)row["CoverImage"];

        return Create(
            name,
            description,
            performerName,
            time,
            status,
            mainImage,
            coverImage,
            null,
            id
        );
    }
}