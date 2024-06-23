using System.Collections.Generic;
using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Application.TodoLists.Queries.GetTodos;
using SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackeUsersProjects;
using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects
{
    public record GetTrackerUsersProjectsQuery : IRequest<List<GetTrackerUsersProjectsDto>>
    {
        public DateTime? StartDateTime { get; init; }
        public DateTime? EndDateTime { get; init; }
        public string? StartDate { get; init; }
        public string? EndDate { get; init; }
        public GetTrackerUsersProjectsQuery(string? startDate, string? endDate)
        {
            StartDateTime = !string.IsNullOrEmpty(startDate) ? DateTime.Parse(startDate, null, System.Globalization.DateTimeStyles.RoundtripKind).Date : null;
            EndDateTime = !string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate, null, System.Globalization.DateTimeStyles.RoundtripKind).Date : null;
        }

    }

    public class GetTrackerUsersProjectsQueryHandler : IRequestHandler<GetTrackerUsersProjectsQuery, List<GetTrackerUsersProjectsDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetTrackerUsersProjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetTrackerUsersProjectsDto>> Handle(GetTrackerUsersProjectsQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.GetTopUsersAndProjectsAsync(request.StartDateTime, request.EndDateTime, cancellationToken);

            var query = result.AsQueryable()
                .ProjectTo<GetTrackerUsersProjectsDto>(_mapper.ConfigurationProvider);

            List<GetTrackerUsersProjectsDto> projectDtos;
            try
            {
                
                projectDtos = query.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception, handle it, etc.
                throw new Exception("An error occurred while retrieving the data.", ex);
            }

            return projectDtos;
        }
    }
}
