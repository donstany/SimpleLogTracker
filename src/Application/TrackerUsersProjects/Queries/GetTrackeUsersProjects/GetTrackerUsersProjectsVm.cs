using SimpleLogTracker.Application.Common.Models;

namespace SimpleLogTracker.Application.TodoLists.Queries.GetTodos;

public class GetTrackerUsersProjectsVm
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    //public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = Array.Empty<LookupDto>();

    //public IReadOnlyCollection<TodoListDto> Lists { get; init; } = Array.Empty<TodoListDto>();
}
