using SimpleLogTracker.Application.TrackerUsers.Queries.GetUsersWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace SimpleLogTracker.Web.Endpoints;

public class TrackerUsers : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(GetUsersWithPagination);
    }

    public async Task<IResult> GetUsersWithPagination(ISender sender, [FromBody] GetUsersForDataTablesQuery query)
    {
        var (users, totalCount) = await sender.Send(query);
        return Results.Ok(new
        {
            draw = query.Draw,
            recordsTotal = totalCount,
            recordsFiltered = totalCount,
            data = users
        });
    }
}
