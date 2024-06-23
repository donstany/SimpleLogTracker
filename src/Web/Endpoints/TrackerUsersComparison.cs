using SimpleLogTracker.Application.TrackerUsersComparison.Queries;

namespace SimpleLogTracker.Web.Endpoints;

public class TrackerUsersComparison : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTrackerUsersComparison,"{userId}"); // TrackerUsersComparison/144
    }

    public async Task<GetUserComparisonDto> GetTrackerUsersComparison(ISender sender, int userId)
    {
        return await sender.Send(new GetUserComparisonQuery { UserId = userId });
    }
}
