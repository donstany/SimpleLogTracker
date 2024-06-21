// Ignore Spelling: Orderable

namespace SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination
{
    public record GetUsersForDataTables : IRequest<PaginatedList<UsersForDataTablesDto>>
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


    public class GetUsersForDataTablesHandler : IRequestHandler<GetUsersForDataTables, PaginatedList<UsersForDataTablesDto>>
    {
        private readonly IMapper _mapper;

        private static readonly List<User> Users = new List<User>
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", CreatedDate = new DateTime(2023, 1, 1), TotalHours = 40 },
            new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", CreatedDate = new DateTime(2023, 2, 1), TotalHours = 35 },
            new User { Id = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 4, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 5, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 6, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 7, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 8, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 9, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 10, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
            new User { Id = 11, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
        };

        public GetUsersForDataTablesHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PaginatedList<UsersForDataTablesDto>> Handle(GetUsersForDataTables request, CancellationToken cancellationToken)
        {
            var query = Users.AsQueryable();

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= request.StartDate.Value && x.CreatedDate <= request.EndDate.Value);
            }

            //if (request.Order.Any())
            //{
            //    var order = request.Order.FirstOrDefault();
            //    var columnName = request.Columns[order!.Column].Data;

            //    query = order.Dir.ToLower() == "asc"
            //        ? query.OrderByDynamic(columnName)
            //        : query.OrderByDescendingDynamic(columnName);
            //}

            var mappedQuery = query
                .Skip(request.Start)
                .Take(request.Length)
                .Select(user => _mapper.Map<UsersForDataTablesDto>(user));

            var totalRecords = query.Count();

            return new PaginatedList<UsersForDataTablesDto>(await Task.FromResult(mappedQuery.ToList()), totalRecords, request.Start / request.Length + 1, request.Length);
        }
    }

    //public static class IQueryableExtensions
    //{
    //    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember)
    //    {
    //        return query.OrderBy(x => EF.Property<object>(x, orderByMember));
    //    }

    //    public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> query, string orderByMember)
    //    {
    //        return query.OrderByDescending(x => EF.Property<object>(x, orderByMember));
    //    }
    //}

    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalHours { get; set; }
    }

    public class UsersForDataTablesDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public int TotalHours { get; set; }

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
