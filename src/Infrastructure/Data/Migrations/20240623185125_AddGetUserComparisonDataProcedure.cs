using Microsoft.EntityFrameworkCore.Migrations;
using static System.Net.Mime.MediaTypeNames;

#nullable disable

namespace SimpleLogTracker.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGetUserComparisonDataProcedure : Migration
    {
        /// <summary>
        /// -- Test with specific userId, startDate, and endDate
        /// EXEC[dbo].[GetUserComparisonData] @userId = 144, @startDate = '2023-09-03', @endDate = '2023-09-03';
        ///-- Test with specific userId and null dates
        ///EXEC[dbo].[GetUserComparisonData] @userId = 144, @startDate = NULL, @endDate = NULL;
        /// </summary>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"
                              CREATE PROCEDURE [dbo].[GetUserComparisonData]
                                    @userId INT,
                                    @startDate DATETIME = NULL,
                                    @endDate DATETIME = NULL
                                AS
                                BEGIN
                                    SET NOCOUNT ON;

                                    SELECT 
                                        u.Id, 
                                        (u.FirstName + ' ' + u.LastName) AS Name, 
                                        ISNULL(ROUND(SUM(t.Hours), 2), 0) AS TotalHours
                                    FROM 
                                        TLUsers u
                                    LEFT JOIN 
                                        TimeLogs t ON u.Id = t.UserId
                                    WHERE 
                                        u.Id = @userId
                                        AND (@startDate IS NULL OR t.Date >= @startDate)
                                        AND (@endDate IS NULL OR t.Date <= @endDate)
                                    GROUP BY 
                                        u.Id, u.FirstName, u.LastName;
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
