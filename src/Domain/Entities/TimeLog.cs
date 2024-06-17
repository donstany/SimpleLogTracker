namespace SimpleLogTracker.Domain.Entities;
public class TimeLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public TLUser? User { get; set; }
    public int ProjectId { get; set; }
    public TLProject? Project { get; set; }
    public DateTime Date { get; set; }
    public float Hours { get; set; }
}
