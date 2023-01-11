namespace TestDotnetAPI.Contracts.Event;

public record CreateEvent(
    string Name,
    string? Performer = "",
    DateTime time,
    string? Status = "Sap dien ra",
    string? Description = "",
    List<Models.Stream>? Streams = new(),
    string? mainImage = "",
    string? coverImage = "",
    List<Models.Attendance>? Attendances = new()
);