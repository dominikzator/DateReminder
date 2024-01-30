using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateReminder
{
    public class ReminderDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Reminder> Reminders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // add IConfigurationRoot  to get connection string 
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("TempDB"));
            }
        }
    }
}
