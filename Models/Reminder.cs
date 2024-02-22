using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Reminder : BaseModel
{
    public static string[] ReminderTypeStrings = new[] { "Non Cyclic", "Weekly", "Monthly", "Annual" };
    public enum ReminderType { NON_CYCLIC, WEEKLY, MONTHLY, ANNUAL}
    public string Title { get; set; }
    public DateTime TargetDate { get; set; }
    public int Priority { get; set; }
    public int SecondsToNotify { get; set; }
    public ReminderType Type { get; set; }
    public bool Reminded { get; set; }

    public User User { get; set; }
    public int UserId { get; set; }
}
