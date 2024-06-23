using System.Reflection;
using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Domain.Entities;
using SimpleLogTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace SimpleLogTracker.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<TLProject> TLProjects => Set<TLProject>();

    public DbSet<TLUser> TLUsers => Set<TLUser>();

    public DbSet<TimeLog> TimeLogs => Set<TimeLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<TLUser>().ToTable("TLUsers");
        builder.Entity<TLProject>().ToTable("TLProjects");
        builder.Entity<TimeLog>().ToTable("TimeLogs");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<TopResult>().HasNoKey().ToView(null);
        builder.Entity<UserWithPagination>().HasNoKey().ToView(null);
        builder.Entity<UserHours>().HasNoKey().ToView(null);
    }

    public async Task<int> InitializeDatabaseAsync(CancellationToken cancellationToken = default)
    {
        int returnValue;

        var connection = Database.GetDbConnection();
        try
        {
            await connection.OpenAsync(cancellationToken);

            using var command = connection.CreateCommand();
            command.CommandText = "[dbo].[InitializeDatabase]";
            command.CommandType = CommandType.StoredProcedure;

            var returnParameter = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            command.Parameters.Add(returnParameter);

            await command.ExecuteNonQueryAsync(cancellationToken);

            returnValue = (int)returnParameter.Value;
        }
        finally
        {
            await connection.CloseAsync();
        }

        return returnValue;
    }

    public async Task<IEnumerable<TopResult>> GetTopResultAsync(DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken)
    {
        var startParam = startDate.HasValue
            ? new SqlParameter("@startDate", startDate.Value)
            : new SqlParameter("@startDate", DBNull.Value);

        var endParam = endDate.HasValue
            ? new SqlParameter("@endDate", endDate.Value)
            : new SqlParameter("@endDate", DBNull.Value);

        var result = await this.Set<TopResult>()
            .FromSqlRaw("EXEC [dbo].[GetTopUsersAndProjects] @startDate, @endDate", startParam, endParam)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<(List<UserWithPagination> Users, int TotalCount)> GetUserWithPaginationAsync(
        DateTime? startDate, DateTime? endDate, int start, int length,
        string orderByColumn, string orderByDirection, CancellationToken cancellationToken)
    {
        var startDateParam = startDate.HasValue
            ? new SqlParameter("@startDate", startDate.Value)
            : new SqlParameter("@startDate", DBNull.Value);

        var endDateParam = endDate.HasValue
            ? new SqlParameter("@endDate", endDate.Value)
            : new SqlParameter("@endDate", DBNull.Value);

        var startParam = new SqlParameter("@start", start);
        var lengthParam = new SqlParameter("@length", length);
        var orderByColumnParam = new SqlParameter("@orderByColumn", orderByColumn);
        var orderByDirectionParam = new SqlParameter("@orderByDirection", orderByDirection);

        var userWithPaginationResult = await this.Set<UserWithPagination>()
            .FromSqlRaw("EXEC [dbo].[GetUsersWithPagination] @startDate, @endDate, @start, @length, @orderByColumn, @orderByDirection",
                startDateParam, endDateParam, startParam, lengthParam, orderByColumnParam, orderByDirectionParam)
            .ToListAsync(cancellationToken);

        var totalCount = userWithPaginationResult.FirstOrDefault()?.TotalCount ?? 0;

        return (userWithPaginationResult, totalCount);
    }


    public async Task<UserHours> GetUserComparisonAsync(int userId, DateTime? startDate, DateTime? endDate, CancellationToken cancellationToken)
    {
        var userIdParam = new SqlParameter("@userId", userId);
        var startDateParam = startDate.HasValue
            ? new SqlParameter("@startDate", startDate.Value)
            : new SqlParameter("@startDate", DBNull.Value);

        var endDateParam = endDate.HasValue
            ? new SqlParameter("@endDate", endDate.Value)
            : new SqlParameter("@endDate", DBNull.Value);
        
        //TODO pass _ instead hardcoded value
        _ = await this.Set<UserHours>()
             .FromSqlRaw("EXEC [dbo].[GetUserComparisonData] @userId, @startDate, @endDate", userIdParam, startDateParam, endDateParam)
             .ToListAsync(cancellationToken);

        return new UserHours() { Id = 4, Name ="Test User", TotalHours = 65.55 };
    }

}
