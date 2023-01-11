namespace TestDotnetAPI.Contracts.Event
{
    public record AllEventResponse(
        int TotalItems,
        int TotalPages,
        int CurrentPage,
        List<EventResponse> Events
    );
}