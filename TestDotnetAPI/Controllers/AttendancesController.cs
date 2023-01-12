namespace TestDotnetAPI.Controllers;

public class AttendancesController : ApiController
{
    AttendancesController()
    {
    }

    [HttpPost]
    public IActionResult CreateAttendance(CreateAttendanceRequest request)
    {
        return Ok("Hello World");
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetAttendance(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpGet("{id:guid}/qr-image/load")]
    public IActionResult GetAttendanceQR(Guid id)
    {
        return Ok("Hello World");
    }

    [HttpPut("{id:guid}")]
    public IActionResult UpdateAttendance(Guid id, UpdateAttendanceRequest request)
    {
        return Ok("Hello World");
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteAttendance(Guid id)
    {
        return Ok("Hello World");
    }
}