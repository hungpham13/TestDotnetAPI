using System.Data;
using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Services.Database;
using TestDotnetAPI.Common.Authentication;
using TestDotnetAPI.Contracts.Event;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Events;
using ErrorOr;

namespace TestDotnetAPI.Controllers;

public class EventsController : ApiController
{
    private readonly IEventService _eventService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly User.VALID_ROLES[] _roles = { Models.User.VALID_ROLES.Admin, Models.User.VALID_ROLES.Manager };
    public EventsController(IEventService eventService, IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _eventService = eventService;
    }

    [HttpPost]
    public IActionResult CreateEvent(CreateEventRequest request, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Event> requestToEventFormat = Event.From(request);
        if (requestToEventFormat.IsError) return Problem(requestToEventFormat.Errors);
        var e = requestToEventFormat.Value;

        ErrorOr<Created> result = _eventService.CreateEvent(e);
        return result.Match(
            created => CreatedAtAction(
                actionName: nameof(GetEvent),
                routeValues: new {id = e.Id},
                value: MapEventResponse(e)),
            errors => Problem(errors)
        );
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEvent(Guid id, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token);
        if (caller.IsError) return Problem(caller.Errors);
        ErrorOr<Event> eventResult = _eventService.GetEvent(id);
        return eventResult.Match(
            e => Ok(MapEventResponse(e)),
            errors => Problem(errors));
    }

    // [HttpGet("?page={page_num:int}&size={page_size:int}")]
    [HttpGet("")]
    public IActionResult GetEvents(int page, int size, string token)
    {
        // validate
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        // get all
        string getTotalSql = $"SELECT COUNT(*) FROM {DatabaseService.EVENT_TABLE}";
        ErrorOr<DataTable> result = DatabaseService.query(getTotalSql);
        if (result.IsError) return Problem(result.Errors);
        var table = result.Value;
        int total = table.AsEnumerable().First().Field<int>(0);

        ErrorOr<List<Event>> eventsResult = _eventService.GetEvents(page, size);
        if (eventsResult.IsError) return Problem(eventsResult.Errors);
        return Ok(new AllEventResponse(
            total,
            size == 0 ? 0 : (int)Math.Ceiling((double)total / size),
            page,
            eventsResult.Value.Select(MapEventResponse).ToList()
        ));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEvent(Guid id, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Deleted> result = _eventService.DeleteEvent(id);
        return result.Match(
            deleted => Ok(),
            errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEvent(Guid id, UpdateEventRequest request, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Event> eventResult = _eventService.GetEvent(id);
        if (eventResult.IsError) return Problem(eventResult.Errors);
        var oldEvent = eventResult.Value;

        ErrorOr<Event> requestToEventFormat = Event.From(id, request, oldEvent);
        if (requestToEventFormat.IsError)
        {
            return Problem(requestToEventFormat.Errors);
        }

        var e = requestToEventFormat.Value;

        ErrorOr<Updated> result = _eventService.UpdateEvent(e);

        return result.Match(
            updated => Ok(MapEventResponse(e)),
            errors => Problem(errors));
    }
    [HttpGet("{eventId:guid}/attendances")]
    public IActionResult GetAllAttendancesEvent(Guid eventId, string token)
    {
        var result = _eventService.GetAttendanceEvent(eventId);
        return result.Match(
            attendances => Ok(attendances.Select(AttendancesController.MapAttendanceResponse).ToList()),
            errors => Problem(errors)
        );
    }

    public static EventResponse MapEventResponse(Event e)
    {
        return new EventResponse(
            e.Id,
            e.Name,
            e.PerformerName,
            e.Time,
            e.Status,
            e.Description,
            e.MainImage,
            e.CoverImage,
            e.Streams?.Select(s => new StreamResponse(s.Name, s.Url, s.File)).ToList()
        );
    }
}