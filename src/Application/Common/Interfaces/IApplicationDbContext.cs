using SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination;
using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<TLProject> TLProjects { get; }
    DbSet<TLUser> TLUsers { get; }
    DbSet<TimeLog> TimeLogs { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> InitializeDatabaseAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TopResult>> GetTopResultAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
    //TODO fix signature
    Task<(List<UserWithPagination> Users, int TotalCount)> GetUserWithPaginationAsync(DateTime? startDate,
                                                                          DateTime? endDate,
                                                                          int start,
                                                                          int length,
                                                                          string orderByColumn,
                                                                          string orderByDirection,
                                                                         CancellationToken cancellationToken);
    Task<UserHours> GetUserComparisonAsync(int userId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);
}
