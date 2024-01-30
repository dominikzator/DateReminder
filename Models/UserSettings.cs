using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UserSettings : BaseModel
{
    public DateTime TimeToNotify { get; set; }
    public DateTime TimeToElapse { get; set; }

    public List<User> Users { get; set; }
}
