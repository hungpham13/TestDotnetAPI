using TestDotnetAPI.Contracts.Attendance;

namespace TestDotnetAPI.Contracts.Event;

public record EventResponse(
    Guid Id,
    string Name,
    string Performer,
    DateTime Time,
    string Status,
    string Description,
    string MainImage,
    string CoverImage,
    List<StreamResponse>? Streams
);