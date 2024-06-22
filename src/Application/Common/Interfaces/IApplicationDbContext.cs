using SimpleLogTracker.Domain.Entities;

namespace SimpleLogTracker.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    DbSet<TLProject> TLProjects { get; }
    DbSet<TLUser> TLUsers { get; }
    DbSet<TimeLog> TimeLogs { get; }
    Task<int> InitializeDatabaseAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TopResult>> GetTopUsersAndProjectsAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
