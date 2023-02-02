namespace TestDotnetAPI.Contracts.Event;
public record StreamResponse(
    string Name,
    string Link,
    string File
);
public record UpdateEventRequest(
    string? Name,
    string? Performer,
    DateTime? Time,
    string? Status,
    string? Description,
    List<StreamResponse>? Streams,
    string? MainImage,
    string? CoverImage
);