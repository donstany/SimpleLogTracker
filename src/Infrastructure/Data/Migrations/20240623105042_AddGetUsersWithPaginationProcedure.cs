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
                        @length INT
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
                        JOIN FilteredLogs f ON u.Id = f.UserId
                        ORDER BY u.Id
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
