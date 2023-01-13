namespace TestDotnetAPI.Contracts.Event
{
    public record Stream(
        string? Name,
        string? Link,
        string? Description
    );
    public record Attendance(
        string Id,
        string userID,
        string eventID,
        DateTime CreatedAt,
        DateTime ModifiedAt,
        string Status,
        string Token
    );
    public record UpdateEventRequest(
        string Name,
        string? Performer,
        DateTime time,
        string? Status,
        string? Description,
        List<Stream>? Streams,
        string? mainImage,
        string? coverImage,
        List<Attendance>? Attendances
    );
}