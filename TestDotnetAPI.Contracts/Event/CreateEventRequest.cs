namespace TestDotnetAPI.Contracts.Event;

public record CreateEventRequest(
    string Name,
    string Performer,
    DateTime time,
    string Status,
    string Description,
    List<Stream> Streams,
    string mainImage,
    string coverImage,
    List<Attendance> Attendances
);