using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.Event;
namespace TestDotnetAPI.Controllers;

public class EventsController : ApiController
{
    public EventsController()
    {
    }

    [HttpPost]
    public IActionResult CreateEvent(CreateEventRequest request)
    {
        return Ok("Hello World");
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetEvent(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpGet("{eventId:guid}/attendances")]
    public IActionResult GetAttendancesEvent(Guid eventId)
    {
        return Ok("Hello World");
    }

    // [HttpGet("?page={page_num:int}&size={page_size:int}")]
    [HttpGet("")]
    public IActionResult GetEvents(int page_num, int page_size)
    {
        return Ok("Hello World");
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEvent(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateEvent(Guid id, UpdateEventRequest request)
    {
        return Ok("Hello World");
    }

}