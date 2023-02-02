using System.Data;
using ErrorOr;
using TestDotnetAPI.Models;
using TestDotnetAPI.ServiceErrors;
using TestDotnetAPI.Services.Attendances;
using TestDotnetAPI.Services.Database;
using TestDotnetAPI.Services.Users;
using Stream = TestDotnetAPI.Models.Stream;


namespace TestDotnetAPI.Services.Events;

public class EventService : IEventService
{
    private readonly IAttendanceService _attendanceService;

    public EventService(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }
    public ErrorOr<Created> CreateEvent(Event e)
    {
        string sql =
            $"INSERT INTO {DatabaseService.EVENT_TABLE} ([Id], [Name], [Performer], [Status], [Description], [MainImage], [CoverImage], [Time]) VALUES" +
            $"('{e.Id}', '{e.Name}', '{e.PerformerName}', '{e.Status}', '{e.Description}', '{e.MainImage}', '{e.CoverImage}', '{e.Time}');";
        var result = DatabaseService.insert(sql);
        if (result.IsError) return result.Errors;
        foreach (var s in e.Streams)
        {
            var sql2 = $"INSERT INTO {DatabaseService.STREAM_TABLE} ([Id], [Name], [Url], [File])" +
                       $"(SELECT '{s.Id}', '{s.Name}', '{s.Url}', '{s.File}');";
            var result2 = DatabaseService.insert(sql2);
            if (result2.IsError) return result2.Errors;

            var sql3 = $"INSERT INTO {DatabaseService.EVENT_STREAM_TABLE} ([StreamId], [EventId])" +
                       $"(SELECT '{s.Id}', '{e.Id}');";
            var result3 = DatabaseService.insert(sql3);
            if (result3.IsError) return result3.Errors;
        }

        return Result.Created;
    }

    public ErrorOr<Deleted> DeleteEvent(Guid id)
    {
        string sql1 = $"DELETE FROM {DatabaseService.STREAM_TABLE} WHERE [Id] = ANY(SELECT [StreamId] FROM {DatabaseService.EVENT_STREAM_TABLE} WHERE [EventId] = '{id}');";
        var result1 = DatabaseService.delete(sql1);
        if (result1.IsError) return result1.Errors;
        
        string sql = $"DELETE FROM {DatabaseService.EVENT_TABLE} WHERE Id='{id}';";
        return DatabaseService.delete(sql);
    }

    public ErrorOr<Event> GetEvent(Guid id)
    {
        string sql1 = $"SELECT * FROM {DatabaseService.STREAM_TABLE} WHERE [Id] = ANY(SELECT [StreamId] FROM {DatabaseService.EVENT_STREAM_TABLE} WHERE [EventId] = '{id}');";
        ErrorOr<DataTable> result1 = DatabaseService.query(sql1);
        if (result1.IsError) return result1.Errors;
        var streamTable = Stream.From(result1.Value);
        if (streamTable.IsError) return streamTable.Errors;

        
        string sql = $"SELECT * FROM {DatabaseService.EVENT_TABLE} WHERE Id = '{id}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        if (table.Rows.Count > 0)
            return Event.From(table.AsEnumerable().First(), streamTable.Value);
        
        return Errors.Event.NotFound;
    }

    public ErrorOr<Updated> UpdateEvent(Event e)
    {
        string sql = $"UPDATE {DatabaseService.EVENT_TABLE} SET " +
            $"[Name] = '{e.Name}', " +
            $"[Performer] = '{e.PerformerName}', " +
            $"[Status] = '{e.Status}', " +
            $"[Description] = '{e.Description}', " +
            $"[MainImage] = {e.MainImage}, " +
            $"[CoverImage] = '{e.CoverImage}', " +
            $"[Time] = '{e.Time}' " +
            $"WHERE Id = '{e.Id}';";
        return DatabaseService.update(sql);
    }
    public ErrorOr<List<Event>> GetEvents(int page, int size)
    {
        List<Event> events = new();
        string sql = "SELECT [Id] FROM (" +
            "SELECT ROW_NUMBER() OVER (ORDER BY [Time] DESC) AS rownumber, * " +
            $"FROM {DatabaseService.EVENT_TABLE})" +
            $"WHERE rownumber <= {size * page} and rownumber > {size * (page - 1)};";
        ErrorOr<DataTable> result = DatabaseService.query(sql);
        if (result.IsError) return result.Errors;

        var table = result.Value;

        foreach (DataRow row in table.Rows)
        {
            var e = GetEvent(Guid.Parse(row["Id"].ToString()));
            if (e.IsError) return e.Errors;
            events.Add(e.Value);
        }
        return events;
    }

    public ErrorOr<List<Attendance>> GetAttendanceEvent(Guid id)
    {
        List<Attendance> attendances = new();
        // string sql = $"SELECT * FROM {DatabaseService.ATTENDANCE_TABLE} AS A" +
        //               $"INNER JOIN {DatabaseService.ACCOUNT_TABLE} U on U.Id = A.UserId" +
        //               $"INNER JOIN {DatabaseService.EVENT_TABLE} E on E.Id = A.EventId" +
        //               $"WHERE [EventId] = '{id}';";
        
        string sql = $"SELECT * FROM {DatabaseService.ATTENDANCE_TABLE} WHERE [EventId] = '{id}';";
        ErrorOr<DataTable> result = DatabaseService.query(sql);
        if (result.IsError) return result.Errors;

        var attendanceTable = result.Value;
        foreach (DataRow row in attendanceTable.Rows)
        {
            var attendance = _attendanceService.GetAttendance(Guid.Parse(row["Id"].ToString()));
            if (attendance.IsError) return attendance.Errors;
            attendances.Add(attendance.Value);
        }
        return attendances;
    }
}