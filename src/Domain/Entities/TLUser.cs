
namespace SimpleLogTracker.Domain.Entities;
public class TLUser
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public ICollection<TimeLog>? TimeLogs { get; set; }
}
