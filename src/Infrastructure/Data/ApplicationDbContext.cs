using System.Reflection;
using SimpleLogTracker.Application.Common.Interfaces;
using SimpleLogTracker.Domain.Entities;
using SimpleLogTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
}
