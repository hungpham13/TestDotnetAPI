using TestDotnetAPI.Models;
using ErrorOr;

namespace TestDotnetAPI.Services.Attendances;

public interface IAttendanceService
{
    public ErrorOr<Attendance> GetAttendance(Guid id);
    public ErrorOr<Created> AddAttendance(Attendance attendance);
    public ErrorOr<Deleted> DeleteAttendance(Guid id);
    public ErrorOr<Updated> UpdateAttendance(Attendance attendance);
    public ErrorOr<byte[]> GetAttendanceQR(Guid id);
}