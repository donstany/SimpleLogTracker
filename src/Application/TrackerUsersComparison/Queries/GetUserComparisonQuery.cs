using SimpleLogTracker.Application.Common.Interfaces;


namespace SimpleLogTracker.Application.TrackerUsersComparison.Queries;
public record GetUserComparisonQuery : IRequest<GetUserComparisonDto>
{
    public int UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
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
        var result = await _context.GetUserComparisonAsync(request.UserId, request.StartDate, request.EndDate, cancellationToken);
        return _mapper.Map<GetUserComparisonDto>(result);
    }
}
