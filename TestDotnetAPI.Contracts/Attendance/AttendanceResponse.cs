using TestDotnetAPI.Contracts.Event;
using TestDotnetAPI.Contracts.User;

namespace TestDotnetAPI.Contracts.Attendance;

public record AttendanceResponse(
    Guid Id,
    UserResponse User,
    EventResponse Event,
    DateTime CreateAt,
    DateTime ModifiedAt,
    string Status
);