using SimpleLogTracker.Application.TrackerUsersComparison.Queries;

namespace SimpleLogTracker.Web.Endpoints;

public class TrackerUsersComparison : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTrackerUsersComparison);
    }

    public async Task<GetUserComparisonDto> GetTrackerUsersComparison(ISender sender, int userId, DateTime? startDate, DateTime? endDate)
    {
        //TODO: pass DateTime? startDate, DateTime? endDate
        return await sender.Send(new GetUserComparisonQuery { UserId = userId });
    }
}
