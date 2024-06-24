using SimpleLogTracker.Application.TrackerUsersComparison.Queries;
using Microsoft.AspNetCore.Mvc;

namespace SimpleLogTracker.Web.Endpoints;

public class TrackerUsersComparison : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTrackerUsersComparison);
    }

    public async Task<GetUserComparisonDto> GetTrackerUsersComparison(ISender sender, [FromQuery] int userId, [FromQuery] string? startDate, [FromQuery] string? endDate)
    {
        DateTime? startDateTime = null;
        DateTime? endDateTime = null;

        if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStartDate))
        {
            startDateTime = parsedStartDate;
        }

        if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEndDate))
        {
            endDateTime = parsedEndDate;
        }

        var query = new GetUserComparisonQuery(startDateTime, endDateTime, userId);
        return await sender.Send(query);
    }
}
