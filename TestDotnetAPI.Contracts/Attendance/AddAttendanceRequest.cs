namespace TestDotnetAPI.Contracts.Attendance;

public record AddAttendanceRequest(
    Guid UserId,
    Guid EventId
);