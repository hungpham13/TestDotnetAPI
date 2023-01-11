using TestDotnetAPI.Models;
namespace TestDotnetAPI.Contracts.Event;

public record EventResponse(
    Guid Id,
    string Name,
    string Performer,
    DateTime time,
    string Status,
    string Description,
    List<Models.Stream> Streams,
    string mainImage,
    string coverImage,
    List<Models.Attendance> Attendances
);