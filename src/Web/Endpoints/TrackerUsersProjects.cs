using SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects;
using SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackeUsersProjects;

namespace SimpleLogTracker.Web.Endpoints;

public class TrackerUsersProjects : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetTrackerUsersProjects);
    }

    public async Task<List<GetTrackerUsersProjectsDto>> GetTrackerUsersProjects(ISender sender, [AsParameters] GetTrackerUsersProjectsQuery request)
    {
        return await sender.Send(request);

    }
}
