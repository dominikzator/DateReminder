using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserSettings : BaseModel
{
    public int SecondsToNotify { get; set; }
    public int SecondsToElapse { get; set; }

    public List<User> Users { get; set; }

    public static int GetDefaultSecondsToNotify() => 10 * 24 * 3600;
    public static int GetDefaultSecondsToElapse() => 2 * 24 * 3600;
}
