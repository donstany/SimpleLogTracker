namespace SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination
{
    public record GetUsersForDataTables : IRequest<PaginatedList<UsersForDataTablesVm>>
    {
        public int Draw { get; init; }
        public int Start { get; init; }
        public int Length { get; init; }
        public List<Column>? Columns { get; init; }
        public List<Order>? Order { get; init; }
        public Search? Search { get; init; }
        public DateTime? StartDate { get; init; }
        public DateTime? EndDate { get; init; }
    }

    public class Column
    {
        public string? Data { get; set; }
        public string? Name { get; set; }
        //public bool Searchable { get; set; }
        //public bool Orderable { get; set; }
        public Search? Search { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string? Dir { get; set; }
    }

    public class Search
    {
        public string? Value { get; set; }
        public bool Regex { get; set; }
    }

    public class GetUsersForDataTablesHandler : IRequestHandler<GetUsersForDataTables, PaginatedList<UsersForDataTablesVm>>
    {
        private readonly IMapper _mapper;

        private static readonly List<User> Users = new List<User>
        {
            new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", CreatedDate = new DateTime(2023, 1, 1), TotalHours = 40 },
            new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", CreatedDate = new DateTime(2023, 2, 1), TotalHours = 35 },
            new User { Id = 3, FirstName = "Bob", LastName = "Johnson", Email = "bob.johnson@example.com", CreatedDate = new DateTime(2023, 3, 1), TotalHours = 45 },
        };

        public GetUsersForDataTablesHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<PaginatedList<UsersForDataTablesVm>> Handle(GetUsersForDataTables request, CancellationToken cancellationToken)
        {
            var query = Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search?.Value))
            {
                query = query.Where(x => x.FirstName!.Contains(request.Search.Value) || x.LastName!.Contains(request.Search.Value) || x.Email!.Contains(request.Search.Value));
            }

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= request.StartDate.Value && x.CreatedDate <= request.EndDate.Value);
            }

            foreach (var column in request.Columns!)
            {
                if (!string.IsNullOrEmpty(column.Search?.Value))
                {
                    switch (column.Data)
                    {
                        case "firstName":
                            query = query.Where(x => x.FirstName!.Contains(column.Search.Value));
                            break;
                        case "lastName":
                            query = query.Where(x => x.LastName!.Contains(column.Search.Value));
                            break;
                        case "email":
                            query = query.Where(x => x.Email!.Contains(column.Search.Value));
                            break;
                        case "totalHours":
                            if (int.TryParse(column.Search.Value, out var totalHours))
                            {
                                query = query.Where(x => x.TotalHours == totalHours);
                            }
                            break;
                    }
                }
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
                .Select(user => _mapper.Map<UsersForDataTablesVm>(user));

            var totalRecords = query.Count();

            return new PaginatedList<UsersForDataTablesVm>(await Task.FromResult(mappedQuery.ToList()), totalRecords, request.Start / request.Length + 1, request.Length);
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

    public class UsersForDataTablesVm
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
                CreateMap<User, UsersForDataTablesVm>();
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
