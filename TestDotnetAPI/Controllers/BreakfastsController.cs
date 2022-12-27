using Microsoft.AspNetCore.Mvc;
using TestDotnetAPI.Contracts.Breakfast;
using TestDotnetAPI.Models;
using TestDotnetAPI.Services.Breakfasts;
using ErrorOr;

namespace TestDotnetAPI.Controllers;


public class BreakfastsController : ApiController
{
    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }


    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        // transfer to model format
        ErrorOr<Breakfast> requestToBreakfastFormat = Breakfast.From(request);
        if (requestToBreakfastFormat.IsError)
        {
            return Problem(requestToBreakfastFormat.Errors);
        }
        var breakfast = requestToBreakfastFormat.Value;

        // save breakfast to database
        ErrorOr<Created> result = _breakfastService.CreateBreakfast(breakfast);

        // transfer to response format
        return result.Match(
            created => CreatedAsGetBreakfast(breakfast),
            errors => Problem(errors));
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfasts(Guid id)
    {
        ErrorOr<Breakfast> breakfastResult = _breakfastService.GetBreakfast(id);
        return breakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors));
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpdateBreakfast(Guid id, UpdateBreakfastRequest request)
    {
        ErrorOr<Breakfast> requestToBreakfastFormat = Breakfast.From(id, request);
        if (requestToBreakfastFormat.IsError)
        {
            return Problem(requestToBreakfastFormat.Errors);
        }

        var breakfast = requestToBreakfastFormat.Value;

        ErrorOr<UpdatedBreakfast> result = _breakfastService.UpdateBreakfast(breakfast);

        return result.Match(
            updatedBreakfast => updatedBreakfast.isNewlyCreated
                ? CreatedAsGetBreakfast(breakfast)
                : NoContent(),
            errors => Problem(errors));
        //TODO: return 201 if a new breakfast is created
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
        ErrorOr<Deleted> result = _breakfastService.DeleteBreakfast(id);
        return result.Match(
            deleted => NoContent(),
            errors => Problem(errors));
    }
    private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
            breakfast.Id,
            breakfast.Name,
            breakfast.Description,
            breakfast.StartDateTime,
            breakfast.EndDateTime,
            breakfast.LastModifiedDateTime,
            breakfast.Savory,
            breakfast.Sweet);
    }

    private IActionResult CreatedAsGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
                actionName: nameof(GetBreakfasts),
                routeValues: new { id = breakfast.Id },
                value: MapBreakfastResponse(breakfast));
    }
}