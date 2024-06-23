namespace SimpleLogTracker.Domain.Entities;
public class UserWithPagination
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public decimal TotalHours { get; set; }
    public int TotalCount { get; set; }
}
