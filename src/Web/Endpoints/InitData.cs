// Ignore Spelling: app

using SimpleLogTracker.Application.InitData.Commands.CreateInitData;

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
