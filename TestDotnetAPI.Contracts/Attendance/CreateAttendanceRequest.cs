namespace TestDotnetAPI.Contracts.Attendance;

public record CreateAttendanceRequest(
    Guid UserId,
    Guid EventId
);