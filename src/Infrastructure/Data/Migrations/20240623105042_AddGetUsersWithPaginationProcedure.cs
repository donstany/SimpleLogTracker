using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleLogTracker.Infrastructure.Data.Migrations
{
    /// <summary>
    /// TEST GetUsersWithPagination
    /// 
    /// EXEC [dbo].[GetUsersWithPagination]
    /// @startDate = '2024-06-01',
    /// @endDate = '2024-06-30',
    /// @start = 0,
    /// @length = 10;  
    /// 
    /// EXEC [dbo].[GetUsersWithPagination]
    /// @startDate = '2024-06-01',
    /// @endDate = NULL,
    /// @start = 0,
    /// @length = 10;
    ///
    /// EXEC [dbo].[GetUsersWithPagination]
    /// @startDate = NULL,
    /// @endDate = NULL,
    /// @start = 10,
    /// @length = 20;
    /// /// </summary>
    public partial class AddGetUsersWithPaginationProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"
                     CREATE PROCEDURE [dbo].[GetUsersWithPagination]
                        @startDate DATETIME = NULL,
                        @endDate DATETIME = NULL,
                        @start INT,
                        @length INT,
                        @orderByColumn NVARCHAR(50) = 'Id',
                        @orderByDirection NVARCHAR(4) = 'ASC'
                    AS
                    BEGIN
                        ;WITH FilteredLogs AS (
                            SELECT UserId, SUM(Hours) AS TotalHours
                            FROM TimeLogs
                            WHERE (@startDate IS NULL OR Date >= @startDate)
                              AND (@endDate IS NULL OR Date <= @endDate)
                            GROUP BY UserId
                        )
                        SELECT 
                            u.Id, 
                            u.FirstName, 
                            u.LastName, 
                            u.Email, 
                            ISNULL(f.TotalHours, 0) AS TotalHours
                        FROM TLUsers u
                        LEFT JOIN FilteredLogs f ON u.Id = f.UserId
                        ORDER BY 
                            CASE WHEN @orderByColumn = 'FirstName' AND @orderByDirection = 'ASC' THEN u.FirstName END ASC,
                            CASE WHEN @orderByColumn = 'FirstName' AND @orderByDirection = 'DESC' THEN u.FirstName END DESC,
                            CASE WHEN @orderByColumn = 'LastName' AND @orderByDirection = 'ASC' THEN u.LastName END ASC,
                            CASE WHEN @orderByColumn = 'LastName' AND @orderByDirection = 'DESC' THEN u.LastName END DESC,
                            CASE WHEN @orderByColumn = 'Email' AND @orderByDirection = 'ASC' THEN u.Email END ASC,
                            CASE WHEN @orderByColumn = 'Email' AND @orderByDirection = 'DESC' THEN u.Email END DESC,
                            CASE WHEN @orderByColumn = 'TotalHours' AND @orderByDirection = 'ASC' THEN ISNULL(f.TotalHours, 0) END ASC,
                            CASE WHEN @orderByColumn = 'TotalHours' AND @orderByDirection = 'DESC' THEN ISNULL(f.TotalHours, 0) END DESC,
                            CASE WHEN @orderByColumn = 'Id' AND @orderByDirection = 'ASC' THEN u.Id END ASC,
                            CASE WHEN @orderByColumn = 'Id' AND @orderByDirection = 'DESC' THEN u.Id END DESC
                        OFFSET @start ROWS
                        FETCH NEXT @length ROWS ONLY;
                    END;
                    GO";

            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string dropProcedure = @"
        DROP PROCEDURE [dbo].[GetUsersWithPagination]";

            migrationBuilder.Sql(dropProcedure);
        }
    }
}
