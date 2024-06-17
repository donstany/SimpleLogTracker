using SimpleLogTracker.Domain.Events;
using Microsoft.Extensions.Logging;

namespace SimpleLogTracker.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;

    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SimpleLogTracker Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
