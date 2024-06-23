using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects;

public class GetTrackerUsersProjectsDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? TotalHours { get; set; }
    public string? Type { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TopResult, GetTrackerUsersProjectsDto>();

        }
    }
}
