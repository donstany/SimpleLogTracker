using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLogTracker.Domain.Entities;
public class TLProject
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<TimeLog> TimeLogs { get; private set; } = new List<TimeLog>();
}
