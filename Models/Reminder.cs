using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Reminder : BaseModel
{
    public DateTime TargetDate { get; set; }
    public int Priority { get; set; }
    public DateTime TimeToNotify { get; set; }
    public DateTime TimeToElapse { get; set; }
    public string? MessageNote { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }
}
