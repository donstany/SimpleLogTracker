using SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackeUsersProjects;

namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects
{
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
                new GetTrackerUsersProjectsDto { Id = 1, Name = "John Doe", Hours = 40, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 2, Name = "Jane Smith", Hours = 35, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 3, Name = "Mark Johnson", Hours = 60, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 4, Name = "Lisa Brown", Hours = 50, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 5, Name = "Tom Hanks", Hours = 45, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 6, Name = "Sara Connor", Hours = 30, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 7, Name = "David Beckham", Hours = 55, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 8, Name = "Eva Green", Hours = 25, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 9, Name = "Frank Ocean", Hours = 20, Type = "user" },
                new GetTrackerUsersProjectsDto { Id = 10, Name = "Grace Kelly", Hours = 15, Type = "user" }
            };

            var projectHours = new List<GetTrackerUsersProjectsDto>
            {
                new GetTrackerUsersProjectsDto { Id = 11, Name = "My own", Hours = 150, Type = "project" },
                new GetTrackerUsersProjectsDto { Id = 12, Name = "Free Time", Hours = 200, Type = "project" },
                new GetTrackerUsersProjectsDto { Id = 13, Name = "Work", Hours = 300, Type = "project" }
            };

            var result = userHours.Concat(projectHours).ToList();
            return await Task.FromResult(result);
        }
    }
}
