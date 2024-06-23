using AutoMapper;
using MediatR;
using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination
{
    public record GetUsersForDataTablesQuery : IRequest<(List<UsersForDataTablesDto> Users, int TotalCount)>
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

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<User, UsersForDataTablesDto>();
                CreateMap<UserWithPagination, UsersForDataTablesDto>();
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
