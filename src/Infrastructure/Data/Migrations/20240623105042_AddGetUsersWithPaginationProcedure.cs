using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleLogTracker.Infrastructure.Data.Migrations
{
    /// <summary>
    ///-- Test Case 1: Basic pagination without filtering or ordering
    ///EXEC GetUsersWithPagination @start = 0, @length = 10;
    ///
    ///-- Test Case 2: Filter by date range
    ///EXEC GetUsersWithPagination @startDate = '2024-01-01', @endDate = '2024-01-02', @start = 0, @length = 10;
    ///
    ///-- Test Case 3: Order by FirstName in descending order
    ///EXEC GetUsersWithPagination @start = 0, @length = 10, @orderByColumn = 'FirstName', @orderByDirection = 'DESC';
    ///
    ///-- Test Case 4: Paginate with different start and length
    ///EXEC GetUsersWithPagination @start = 1, @length = 1;
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
