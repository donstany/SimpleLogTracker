// Ignore Spelling: app

using SimpleLogTracker.Application.Common.Models;
using SimpleLogTracker.Application.TodoItems.Commands.CreateTodoItem;
using SimpleLogTracker.Application.TodoItems.Commands.DeleteTodoItem;
using SimpleLogTracker.Application.TodoItems.Commands.UpdateTodoItem;
using SimpleLogTracker.Application.TodoItems.Commands.UpdateTodoItemDetail;
using SimpleLogTracker.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace SimpleLogTracker.Web.Endpoints;

public class InitData : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(SeedInitData);
    }

    public Task<int> SeedInitData(ISender sender, CreateInitDataCommand command)
    {
        return sender.Send(command);
    }
}
