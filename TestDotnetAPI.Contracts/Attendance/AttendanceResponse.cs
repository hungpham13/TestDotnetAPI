namespace TestDotnetAPI.Contracts.Attendance;

public record AttendanceResponse(
    Guid Id,
    Guid UserId,
    Guid EventId,
    DateTime CreateAt,
    DateTime LastModifiedAt,
    string Status,
    string token
);