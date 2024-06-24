using SimpleLogTracker.Application.Common.Interfaces;

namespace SimpleLogTracker.Application.TrackerUsersComparison.Queries;

public record GetUserComparisonQuery : IRequest<GetUserComparisonDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int UserId { get; set; }

    public GetUserComparisonQuery(DateTime? startDate, DateTime? endDate, int userId)
    {
        StartDate = startDate;
        EndDate = endDate;
        UserId = userId;
    }
}

public class GetUserComparisonDataQueryHandler : IRequestHandler<GetUserComparisonQuery, GetUserComparisonDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetUserComparisonDataQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetUserComparisonDto> Handle(GetUserComparisonQuery request, CancellationToken cancellationToken)
    {
        var getUserComparisonAsyncResult = await _context.GetUserComparisonAsync(request.UserId, request.StartDate, request.EndDate, cancellationToken);
        return _mapper.Map<GetUserComparisonDto>(getUserComparisonAsyncResult);
    }
}
