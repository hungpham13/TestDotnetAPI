using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using TestDotnetAPI.Common.Authentication;
using TestDotnetAPI.Contracts.Attendance;
using TestDotnetAPI.Models;
using TestDotnetAPI.ServiceErrors;
using TestDotnetAPI.Services.Attendances;
using TestDotnetAPI.Services.Events;
using TestDotnetAPI.Services.Users;

namespace TestDotnetAPI.Controllers;

public class AttendancesController : ApiController
{
    private readonly IUserService _userService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEventService _eventService;
    private readonly IAttendanceService _attendanceService;
    private readonly User.VALID_ROLES[] _roles = { Models.User.VALID_ROLES.Admin, Models.User.VALID_ROLES.Manager };
    AttendancesController(IEventService eventService, IUserService userService, IJwtTokenGenerator jwtTokenGenerator, IAttendanceService attendanceService)
    {
        _eventService = eventService;
        _userService = userService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _attendanceService = attendanceService;
    }

    [HttpPost]
    public IActionResult CreateAttendance(CreateAttendanceRequest request, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        var user = _userService.GetUser(request.UserId);
        if (user.IsError) return Problem(user.Errors);
        
        var e = _eventService.GetEvent(request.EventId);
        if (e.IsError) return Problem(e.Errors);
        
        var requestToAttendanceFormat = Attendance.From(user.Value, e.Value);
        if (requestToAttendanceFormat.IsError) return Problem(requestToAttendanceFormat.Errors);
        
        var attendance = requestToAttendanceFormat.Value;
        
        var result = _attendanceService.AddAttendance(attendance);
        return result.Match(
            created => CreatedAtAction( 
                actionName: nameof(GetAttendance), 
                routeValues: new {id = attendance.Id}, 
                value: MapAttendanceResponse(attendance)),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetAttendance(Guid id, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token);
        if (caller.IsError) return Problem(caller.Errors);
        ErrorOr<Attendance> attendanceResult = _attendanceService.GetAttendance(id);
        return attendanceResult.Match(
            a => Ok(MapAttendanceResponse(a)),
            errors => Problem(errors));
    }

    [HttpGet("{id:guid}/qr-image/load")]
    public IActionResult GetAttendanceQR(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateAttendance(Guid id, UpdateAttendanceRequest request, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Attendance> attendanceResult = _attendanceService.GetAttendance(id);
        if (attendanceResult.IsError) return Problem(attendanceResult.Errors);
        var oldAttendance = attendanceResult.Value;

        ErrorOr<Attendance> requestToAttendanceFormat = Attendance.From(id, request, oldAttendance);
        if (requestToAttendanceFormat.IsError)
        {
            return Problem(requestToAttendanceFormat.Errors);
        }

        var a = requestToAttendanceFormat.Value;

        ErrorOr<Updated> result = _attendanceService.UpdateAttendance(a);

        return result.Match(
            updated => Ok(MapAttendanceResponse(a)),
            errors => Problem(errors));
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteAttendance(Guid id, string token)
    {
        ErrorOr<string> caller = _jwtTokenGenerator.ValidateToken(token, _roles);
        if (caller.IsError) return Problem(caller.Errors);
        
        ErrorOr<Deleted> result = _attendanceService.DeleteAttendance(id);
        return result.Match(
            deleted => Ok(),
            errors => Problem(errors));
    }
    public static AttendanceResponse MapAttendanceResponse(Attendance attendance)
    {
        return new AttendanceResponse(
            Id: attendance.Id,
            User: UsersController.MapUserResponse(attendance.User),
            Event: EventsController.MapEventResponse(attendance.Event),
            CreateAt: attendance.CreatedAt,
            ModifiedAt: attendance.ModifiedAt,
            Status: attendance.Status
        );
    }
}