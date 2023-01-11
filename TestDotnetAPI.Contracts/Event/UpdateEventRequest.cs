namespace TestDotnetAPI.Contracts.Event
{
    public record UpdateEventRequest(
        string Name,
        string? Performer,
        DateTime time,
        string? Status,
        string? Description,
        List<Models.Stream>? Streams,
        string? mainImage,
        string? coverImage,
        List<Models.Attendance>? Attendances
    );
}