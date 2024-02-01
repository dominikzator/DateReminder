using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Reminder : BaseModel
{
    [MaxLength(20)]
    public string Title { get; set; }
    public DateTime TargetDate { get; set; }
    public int Priority { get; set; }
    public int SecondsToNotify { get; set; }
    public int SecondsToElapse { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }
}
