using SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackeUsersProjects;

namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects;

public record GetTrackerUsersProjectsQuery : IRequest<List<GetTrackerUsersProjectsDto>>;

public class GetTrackerUsersProjectsQueryHandler : IRequestHandler<GetTrackerUsersProjectsQuery, List<GetTrackerUsersProjectsDto>>
{
    public GetTrackerUsersProjectsQueryHandler()
    {
    }

    public async Task<List<GetTrackerUsersProjectsDto>> Handle(GetTrackerUsersProjectsQuery request, CancellationToken cancellationToken)
    {
        var userHours = new List<GetTrackerUsersProjectsDto>
        {
            new GetTrackerUsersProjectsDto { Name = "John Doe", Hours = 40, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Jane Smith", Hours = 35, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Mark Johnson", Hours = 60, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Lisa Brown", Hours = 50, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Tom Hanks", Hours = 45, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Sara Connor", Hours = 30, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "David Beckham", Hours = 55, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Eva Green", Hours = 25, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Frank Ocean", Hours = 20, Type = "user" },
            new GetTrackerUsersProjectsDto { Name = "Grace Kelly", Hours = 15, Type = "user" }
        };

        var projectHours = new List<GetTrackerUsersProjectsDto>
        {
            new GetTrackerUsersProjectsDto { Name = "My own", Hours = 150, Type = "project" },
            new GetTrackerUsersProjectsDto { Name = "Free Time", Hours = 200, Type = "project" },
            new GetTrackerUsersProjectsDto { Name = "Work", Hours = 300, Type = "project" }
        };

        var result = userHours.Concat(projectHours).ToList();
        return await Task.FromResult(result);
    }
}
