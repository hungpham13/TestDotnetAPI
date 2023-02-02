using TestDotnetAPI.Contracts.Attendance;
namespace TestDotnetAPI.Contracts.Event;

public record AllAttendanceEventResponse(
    List<AttendanceResponse> AttendanceEvents
);