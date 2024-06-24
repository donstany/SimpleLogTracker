using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination.Dtos;
using SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination.ViewModels;

namespace SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination
{
    public record GetUsersForDataTablesQuery : IRequest<(List<UsersForDataTablesDto> Users, int TotalCount)>
    {
        public int Draw { get; init; }
        public int Start { get; init; }
        public int Length { get; init; }
        public List<ColumnVm>? Columns { get; init; }
        public List<OrderVm>? Order { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }

    public class GetUsersForDataTablesHandler : IRequestHandler<GetUsersForDataTablesQuery, (List<UsersForDataTablesDto> Users, int TotalCount)>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetUsersForDataTablesHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<(List<UsersForDataTablesDto> Users, int TotalCount)> Handle(GetUsersForDataTablesQuery request, CancellationToken cancellationToken)
        {
            var orderByColumn = request.Columns?[request.Order?.FirstOrDefault()?.Column ?? 0]?.Data ?? "Id";
            var orderByDirection = request.Order?.FirstOrDefault()?.Dir ?? "ASC";

            var (users, totalCount) = await _context.GetUserWithPaginationAsync(
                request.StartDate,
                request.EndDate,
                request.Start,
                request.Length,
                orderByColumn,
                orderByDirection,
                cancellationToken
            );

            var userDtos = _mapper.Map<List<UsersForDataTablesDto>>(users);

            return (userDtos, totalCount);
        }
    }
}
