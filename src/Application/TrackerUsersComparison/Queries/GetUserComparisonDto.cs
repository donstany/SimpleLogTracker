using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.TrackerUsersComparison.Queries;

public class GetUserComparisonDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double TotalHours { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserHours, GetUserComparisonDto>();
        }
    }
}
