using Microsoft.EntityFrameworkCore;
using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.TrackerUsersComparison.Queries;
public record GetUserComparisonQuery : IRequest<GetUserComparisonDto>
{
    public int UserId { get; set; }
}

public class GetUserComparisonDataQueryHandler : IRequestHandler<GetUserComparisonQuery, GetUserComparisonDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserComparisonDataQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserComparisonDto> Handle(GetUserComparisonQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.GetUserComparisonAsync(request.UserId, DateTime.Now, DateTime.Now, cancellationToken);

        //var user = await _context.TLUsers
        //    .Where(u => u.Id == request.UserId)
        //    .Select(u => new UserComparisonDto
        //    {
        //        Id = u.Id,
        //        Name = u.FirstName + " " + u.LastName,
        //        TotalHours = _context.TimeLogs
        //            .Where(t => t.UserId == u.Id)
        //            .Sum(t => t.Hours)
        //    })
        //    .FirstOrDefaultAsync(cancellationToken);

        //if (user == null)
        //{
        //    throw new NotFoundException(nameof(TLUser), request.UserId);
        //}
        var b = new GetUserComparisonDto();
        return b;
    }
}
