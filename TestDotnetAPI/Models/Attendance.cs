using System.Data;
using ErrorOr;
using TestDotnetAPI.Common.Utils;
using TestDotnetAPI.Contracts.Attendance;
using TestDotnetAPI.ServiceErrors;

namespace TestDotnetAPI.Models;

public class Attendance
{
    public Guid Id { get; }
    public User User { get; }
    public Event Event { get; }
    public DateTime CreatedAt { get; }
    public DateTime ModifiedAt { get; }
    public string Status { get; }
    
    public enum VALID_STATUS { Initialize, Verified };

    private Attendance(
        Guid id,
        User user,
        Event e,
        DateTime createdAt,
        DateTime modifiedAt,
        string status
    )
    {
        Id = id;
        User = user;
        Event = e;
        CreatedAt = createdAt;
        ModifiedAt = modifiedAt;
        Status = status;
    }
    private static ErrorOr<Attendance> Create(
        User user,
        Event e,
        string status,
        DateTime? createdAt = null,
        Guid? id = null
    )
    {
        List<Error> errors = new();
        if (Enum.IsDefined(typeof(VALID_STATUS), status) is false)
        {
            errors.Add(Errors.Attendance.InvalidStatus);
        }
        // if (user == null)
        // {
        //     return new ErrorOr<Attendance>(new UserNotFound());
        // }
        // if (e == null)
        // {
        //     return new ErrorOr<Attendance>(new EventNotFound());
        // }
        // if (status == null)
        // {
        //     return new ErrorOr<Attendance>(new InvalidStatus());
        // }
        if (errors.Count > 0)
        {
            return errors;
        }
        return new Attendance(
            id ?? Guid.NewGuid(),
            user,
            e,
            createdAt ?? DateTime.Now,
            DateTime.Now,
            status
        );
    }

    public static ErrorOr<Attendance> From(User user, Event e)
    {
        return Create(user, e, "Initialize");
    }
    public static ErrorOr<Attendance> From(Guid id, UpdateAttendanceRequest request, Attendance attendance)
    {
        return Create(
            attendance.User,
            attendance.Event,
            request.Status,
            attendance.CreatedAt,
            id
        );
    }
    public static ErrorOr<Attendance> From(DataRow row, User user, Event e)
    {
        return Create(
            user,
            e,
            (string)row["Status"],
            Util.ConvertFromDBVal<DateTime>(row["CreatedAt"]),
            Guid.Parse(row["Id"].ToString())
        );
    }
};