using System.Data;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Database;
using ErrorOr;
using QRCoder;
using TestDotnetAPI.ServiceErrors;
using TestDotnetAPI.Services.Events;
using TestDotnetAPI.Services.Users;

namespace TestDotnetAPI.Services.Attendances;

public class AttendanceService : IAttendanceService
{
    
    private readonly IUserService _userService;
    private readonly IEventService _eventService;
    
    public AttendanceService(IUserService userService, IEventService eventService)
    {
        _userService = userService;
        _eventService = eventService;
    }
    public ErrorOr<Created> AddAttendance(Attendance attendance)
    {
        string sql = $"INSERT INTO {DatabaseService.ATTENDANCE_TABLE} ([Id], [UserId], [EventId]) VALUES" +
                     $"('{Guid.NewGuid()}', '{attendance.User.Id}', '{attendance.Event.Id}');";
        return DatabaseService.insert(sql);
    }
    
    public ErrorOr<Attendance> GetAttendance(Guid id)
    {
        string sql = $"SELECT * FROM {DatabaseService.ATTENDANCE_TABLE} WHERE [Id] = '{id}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql);
        if (result.IsError) return result.Errors;

        var attendanceTable = result.Value;
        if (attendanceTable.Rows.Count == 0) return Errors.Attendance.NotFound;
        
        var row = attendanceTable.AsEnumerable().First();
        var e = _eventService.GetEvent(Guid.Parse(row["EventId"].ToString()));
        if (e.IsError) return e.Errors;

        var user = _userService.GetUser(Guid.Parse(row["UserId"].ToString()));
        if (user.IsError) return user.Errors;
        
        return Attendance.From(row, user.Value, e.Value);
    }

    public ErrorOr<byte[]> GetAttendanceQR(Guid id)
    {
        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(id.ToString(), QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        return qrCode.GetGraphic(20);
    }
    public ErrorOr<Deleted> DeleteAttendance(Guid id)
    {
        // _users.Remove(id);
        string sql = $"DELETE FROM {DatabaseService.ATTENDANCE_TABLE} WHERE Id = '{id}';";
        return DatabaseService.delete(sql);
    }
    public ErrorOr<Updated> UpdateAttendance(Attendance attendance)
    {
        string sql = $"UPDATE {DatabaseService.ATTENDANCE_TABLE} SET " +
                     $"[Status] = '{attendance.Status}', " +
                     $"[ModifiedAt] = '{attendance.ModifiedAt}' " +
                     $"[CreatedAt] = '{attendance.CreatedAt}' " +
                     $"WHERE [Id] = '{attendance.Id}';";
        return DatabaseService.update(sql);
    }
}