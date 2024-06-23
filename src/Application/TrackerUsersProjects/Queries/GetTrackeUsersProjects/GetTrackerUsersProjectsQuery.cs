using System.Collections.Generic;
using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Application.TodoLists.Queries.GetTodos;
using SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackeUsersProjects;
using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.TrackerUsersProjects.Queries.GetTrackerUsersProjects
{
    public record GetTrackerUsersProjectsQuery(DateTime? StartDate, DateTime? EndDate) : IRequest<List<GetTrackerUsersProjectsDto>>;

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
            var result = await _context.GetTopUsersAndProjectsAsync(request.StartDate, request.EndDate, cancellationToken);

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
