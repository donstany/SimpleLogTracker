using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleLogTracker.Infrastructure.Data.Migrations
{
    public partial class InitDbStoreProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var procedure = @"
       CREATE PROCEDURE [dbo].[InitializeDatabase] AS

        BEGIN

            DELETE FROM TimeLogs;
            DELETE FROM TLUsers;
            DELETE FROM TLProjects;

            DECLARE @FirstNames TABLE (Name NVARCHAR(50));
            INSERT INTO @FirstNames VALUES ('John'), ('Gringo'), ('Mark'), ('Lisa'), ('Maria'), ('Sonya'), ('Philip'), ('Jose'), ('Lorenzo'), ('George'), ('Justin');

            DECLARE @LastNames TABLE (Surname NVARCHAR(50));
            INSERT INTO @LastNames VALUES ('Johnson'), ('Lamas'), ('Jackson'), ('Brown'), ('Mason'), ('Rodriguez'), ('Roberts'), ('Thomas'), ('Rose'), ('McDonalds');

            DECLARE @Domains TABLE (Domain NVARCHAR(50));
            INSERT INTO @Domains VALUES ('hotmail.com'), ('gmail.com'), ('live.com');

            DECLARE @FirstName NVARCHAR(50), @LastName NVARCHAR(50), @Domain NVARCHAR(50), @Email NVARCHAR(100), @Counter INT = 1;

            WHILE @Counter <= 100
            BEGIN
            SELECT @FirstName = Name FROM @FirstNames ORDER BY NEWID();
            SELECT @LastName = Surname FROM @LastNames ORDER BY NEWID();
            SELECT @Domain = Domain FROM @Domains ORDER BY NEWID();
        
            SET @Email = @FirstName + '.' + @LastName + '@' + @Domain;
        
            INSERT INTO TLUsers (FirstName, LastName, Email)
            VALUES (@FirstName, @LastName, @Email);
        
            SET @Counter = @Counter + 1;
            END

            INSERT INTO TLProjects (Name) VALUES ('My own'), ('Free Time'), ('Work');


            DECLARE @UserId INT, @ProjectId INT, @LogDate DATE, @Hours FLOAT, @DayHours FLOAT;

            DECLARE UserCursor CURSOR FOR SELECT Id FROM TLUsers;
            OPEN UserCursor;
            FETCH NEXT FROM UserCursor INTO @UserId;

            WHILE @@FETCH_STATUS = 0
            BEGIN
            SET @Counter = 1;
            WHILE @Counter <= FLOOR(RAND() * 20) + 1
            BEGIN
            SET @ProjectId = (SELECT TOP 1 Id FROM TLProjects ORDER BY NEWID());
            SET @LogDate = DATEADD(DAY, -1 * FLOOR(RAND() * 365), GETDATE());
            SET @Hours = ROUND(RAND() * 8, 2);
            
            SET @DayHours = (SELECT ISNULL(SUM(Hours), 0) FROM TimeLogs WHERE UserId = @UserId AND Date = @LogDate);
            
            IF @DayHours + @Hours <= 8
            BEGIN
            INSERT INTO TimeLogs  (UserId, ProjectId, Date, Hours)
            VALUES (@UserId, @ProjectId, @LogDate, @Hours);
            END
            
            SET @Counter = @Counter + 1;
            END

            FETCH NEXT FROM UserCursor INTO @UserId;
            END

            CLOSE UserCursor;
            DEALLOCATE UserCursor;
        END;
            ";

            migrationBuilder.Sql(procedure);
        }

    }
}
