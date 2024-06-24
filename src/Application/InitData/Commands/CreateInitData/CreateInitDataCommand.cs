using SimpleLogTracker.Application.Common.Interfaces;

namespace SimpleLogTracker.Application.InitData.Commands.CreateInitData;

public record CreateInitDataCommand : IRequest<int>
{
    public int Id { get; init; }
}

public class CreateInitCommandHandler : IRequestHandler<CreateInitDataCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateInitCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateInitDataCommand request, CancellationToken cancellationToken)
    {
        return await _context.InitializeDatabaseAsync(cancellationToken);
    }
}
