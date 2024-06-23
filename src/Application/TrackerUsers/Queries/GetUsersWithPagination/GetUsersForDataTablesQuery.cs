using SimpleLogTracker.Application.Common.Interfaces;

namespace SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination
{
    public record GetUsersForDataTablesQuery : IRequest<PaginatedList<UsersForDataTablesDto>>
    {
        public int Draw { get; init; }
        public int Start { get; init; }
        public int Length { get; init; }
        public List<Column>? Columns { get; init; }
        public List<Order>? Order { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }

    public class Column
    {
        public string? Data { get; set; }
        public string? Name { get; set; }
        public bool Orderable { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string? Dir { get; set; }
    }

    public class GetUsersForDataTablesHandler : IRequestHandler<GetUsersForDataTablesQuery, PaginatedList<UsersForDataTablesDto>>
    {
        private readonly IMapper _mapper;
        private readonly IApplicationDbContext _context;

        public GetUsersForDataTablesHandler(IMapper mapper, IApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PaginatedList<UsersForDataTablesDto>> Handle(GetUsersForDataTablesQuery request, CancellationToken cancellationToken)
        {
            var orderByColumn = request.Columns?[request.Order?.FirstOrDefault()?.Column ?? 0]?.Data ?? "Id";
            var orderByDirection = request.Order?.FirstOrDefault()?.Dir ?? "ASC";

            var result = await _context.GetUserWithPaginationAsync(
                request.StartDate,
                request.EndDate,
                request.Start,
                request.Length,
                orderByColumn,
                orderByDirection,
                cancellationToken
            );

            return result;
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public int TotalHours { get; set; }
    }

    public class UsersForDataTablesDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public decimal TotalHours { get; set; }
        //public DateTime CreatedDate { get; set; }
        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<User, UsersForDataTablesDto>();
            }
        }
    }

    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalCount { get; }
        public int PageIndex { get; }
        public int PageSize { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }
    }
}
