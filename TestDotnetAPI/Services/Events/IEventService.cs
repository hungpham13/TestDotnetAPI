using ErrorOr;
using TestDotnetAPI.Models;

namespace TestDotnetAPI.Services.Events;

public interface IEventService
{
    public ErrorOr<Created> CreateEvent(Event e);
    public ErrorOr<Deleted> DeleteEvent(Guid id);
    public ErrorOr<Event> GetEvent(Guid id);
    public ErrorOr<Updated> UpdateEvent(Event e);
    public ErrorOr<List<Event>> GetEvents(int page, int size);
    public ErrorOr<List<Attendance>> GetAttendanceEvent(Guid id);
}