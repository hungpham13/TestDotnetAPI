using TestDotnetAPI.Contracts.Attendance;

namespace TestDotnetAPI.Contracts.Event;

public record CreateEventRequest(
    string Name,
    string Performer,
    DateTime Time,
    string Status,
    string Description,
    List<StreamResponse> Streams,
    string MainImage,
    string CoverImage
);