using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleLogTracker.Infrastructure.Data.Migrations
{
    public partial class AddGetTopUsersAndProjectsProcedure : Migration
    {
        /// <summary>
        /// --TEST GetTopUsersAndProjects
        ///-- Call with specific date range
        ///--EXEC[dbo].[GetTopUsersAndProjects] @startDate = '2023-01-01', @endDate = '2023-12-31';

        ///-- Call with only end date
        ///--EXEC[dbo].[GetTopUsersAndProjects] @startDate = NULL, @endDate = '2025-07-17';

        ///-- Call without date filters
        ///--EXEC[dbo].[GetTopUsersAndProjects];
        /// </summary>
        /// <param name="migrationBuilder"></param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"
                      CREATE PROCEDURE [dbo].[GetTopUsersAndProjects]
                        @startDate datetime2 = NULL,
                        @endDate datetime2 = NULL
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        WITH TopUsers AS (
                            SELECT TOP 10 
                                [TL].[UserId] AS Id, 
                                CONCAT([TU].[FirstName], ' ', [TU].[LastName]) AS Name,
                                FORMAT(SUM([TL].[Hours]), 'N2') AS TotalHours, 
                                'User' AS Type
                            FROM [TimeLogs] AS TL
                            JOIN [TLUsers] AS TU ON [TL].[UserId] = [TU].[Id]
                            WHERE (@startDate IS NULL OR [TL].[Date] >= @startDate)
                              AND (@endDate IS NULL OR [TL].[Date] <= @endDate)
                            GROUP BY [TL].[UserId], [TU].[FirstName], [TU].[LastName]
                            ORDER BY SUM([TL].[Hours]) DESC
                        ),

                        TopProjects AS (
                            SELECT TOP 10 
                                [TL].[ProjectId] AS Id, 
                                [TP].[Name] AS Name,
                                FORMAT(SUM([TL].[Hours]), 'N2') AS TotalHours, 
                                'Project' AS Type
                            FROM [TimeLogs] AS TL
                            JOIN [TLProjects] AS TP ON [TL].[ProjectId] = [TP].[Id]
                            WHERE (@startDate IS NULL OR [TL].[Date] >= @startDate)
                              AND (@endDate IS NULL OR [TL].[Date] <= @endDate)
                            GROUP BY [TL].[ProjectId], [TP].[Name]
                            ORDER BY SUM([TL].[Hours]) DESC
                        )

                        SELECT Id, Name, TotalHours, Type 
                        FROM TopUsers
                        UNION ALL
                        SELECT Id, Name, TotalHours, Type 
                        FROM TopProjects
                        ORDER BY TotalHours DESC;
                    END
                    GO";

            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string dropProcedure = @"
        DROP PROCEDURE [dbo].[GetTopUsersAndProjects]";

            migrationBuilder.Sql(dropProcedure);
        }
    }

}
