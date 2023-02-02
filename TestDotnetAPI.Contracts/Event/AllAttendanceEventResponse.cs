namespace TestDotnetAPI.Contracts.Attendance
{
    public record AllAttendanceEventResponse(
        List<AttendanceResponse> AttendanceEvents
    );
}