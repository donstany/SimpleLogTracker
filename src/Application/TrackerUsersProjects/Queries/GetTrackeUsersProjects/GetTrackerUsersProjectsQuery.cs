using SimpleLogTracker.Application.Common.Interfaces;

namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects
{
    public record GetTrackerUsersProjectsQuery : IRequest<IEnumerable<GetTrackerUsersProjectsDto>>
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

    public class GetTrackerUsersProjectsQueryHandler : IRequestHandler<GetTrackerUsersProjectsQuery, IEnumerable<GetTrackerUsersProjectsDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetTrackerUsersProjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetTrackerUsersProjectsDto>> Handle(GetTrackerUsersProjectsQuery request, CancellationToken cancellationToken)
        {
            var getTopResultAsyncResult = await _context.GetTopResultAsync(request.StartDateTime, request.EndDateTime, cancellationToken);

            return getTopResultAsyncResult.AsQueryable()
                                          .ProjectTo<GetTrackerUsersProjectsDto>(_mapper.ConfigurationProvider)
                                          .ToList();
        }
    }
}
