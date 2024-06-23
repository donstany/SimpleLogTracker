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
    ///returned colums Id,FirstName,LastName,Email,TotalHours,TotalCount
    ///Id	FirstName LastName	 Email	                     TotalHours	TotalCount
    ///55	Justin    McDonalds  Justin.McDonalds@gmail.com  11.65	    97
    /// </summary>
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
       
                                    DECLARE @TempTable TABLE (
                                        Id INT,
                                        FirstName NVARCHAR(50),
                                        LastName NVARCHAR(50),
                                        Email NVARCHAR(50),
                                        TotalHours DECIMAL(5, 2)
                                    );

                                    INSERT INTO @TempTable
                                    SELECT 
                                        u.Id, 
                                        u.FirstName, 
                                        u.LastName, 
                                        u.Email, 
                                        ISNULL(f.TotalHours, 0) AS TotalHours
                                    FROM TLUsers u
                                    JOIN (
                                        SELECT UserId, SUM(Hours) AS TotalHours
                                        FROM TimeLogs
                                        WHERE (@startDate IS NULL OR Date >= @startDate)
                                            AND (@endDate IS NULL OR Date <= @endDate)
                                        GROUP BY UserId
                                    ) f ON u.Id = f.UserId;

                                    DECLARE @TotalCount INT;
                                    SELECT @TotalCount = COUNT(*) FROM @TempTable;

                                    SELECT 
                                        Id,
                                        FirstName,
                                        LastName,
                                        Email,
                                        TotalHours,
                                        @TotalCount AS TotalCount
                                    FROM @TempTable
                                    ORDER BY 
                                        CASE WHEN @orderByColumn = 'FirstName' AND @orderByDirection = 'ASC' THEN FirstName END ASC,
                                        CASE WHEN @orderByColumn = 'FirstName' AND @orderByDirection = 'DESC' THEN FirstName END DESC,
                                        CASE WHEN @orderByColumn = 'LastName' AND @orderByDirection = 'ASC' THEN LastName END ASC,
                                        CASE WHEN @orderByColumn = 'LastName' AND @orderByDirection = 'DESC' THEN LastName END DESC,
                                        CASE WHEN @orderByColumn = 'Email' AND @orderByDirection = 'ASC' THEN Email END ASC,
                                        CASE WHEN @orderByColumn = 'Email' AND @orderByDirection = 'DESC' THEN Email END DESC,
                                        CASE WHEN @orderByColumn = 'TotalHours' AND @orderByDirection = 'ASC' THEN TotalHours END ASC,
                                        CASE WHEN @orderByColumn = 'TotalHours' AND @orderByDirection = 'DESC' THEN TotalHours END DESC,
                                        CASE WHEN @orderByColumn = 'Id' AND @orderByDirection = 'ASC' THEN Id END ASC,
                                        CASE WHEN @orderByColumn = 'Id' AND @orderByDirection = 'DESC' THEN Id END DESC
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
