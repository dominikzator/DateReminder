using System;
using System.Collections.Generic;

public partial class User : BaseModel
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public UserSettings UserSettings { get; set; }
    public int UserSettingsId { get; set; }

    public List<Reminder> Reminders { get; set; }
}
