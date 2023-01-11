namespace TestDotnetAPI.Contracts.Attendance
{
    public record AllAttendanceEventResponse(
        List<AttendanceEventResponse> AttendanceEvents
    );
}