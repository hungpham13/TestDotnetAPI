namespace TestDotnetAPI.Contracts.Attendance;

public record AttendanceResponse(
    Guid Id,
    Guid UserId,
    Guid EventId,
    DateTime CreateAt,
    DateTime ModifiedAt,
    string Status,
    string token
);