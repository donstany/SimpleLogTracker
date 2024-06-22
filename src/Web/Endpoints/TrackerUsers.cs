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

    public async Task<IResult> GetUsersWithPagination(ISender sender, [FromBody] GetUsersForDataTables query)
    {
        var result = await sender.Send(query);
        return Results.Ok(new
        {
            draw = query.Draw,
            recordsTotal = result.TotalCount,
            recordsFiltered = result.TotalCount,
            data = result.Items
        });
    }
}
