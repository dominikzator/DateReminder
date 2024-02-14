using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateReminder.Configurations
{
    public class RemindersConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();
            builder.Property(p => p.Title).HasMaxLength(40).IsRequired();

            builder.HasData(
                new Reminder
                {
                    Id = 1,
                    Priority = 4,
                    Title = "Urodziny Sylwii",
                    TargetDate = new DateTime(2024, 08, 21),
                    UserId = 1,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = true,
                },
                new Reminder
                {
                    Id = 2,
                    Priority = 4,
                    Title = "Pierwszy Dzień Wiosny",
                    TargetDate = new DateTime(2024, 03, 21),
                    UserId = 1,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = true,
                },
                new Reminder
                {
                    Id = 3,
                    Priority = 4,
                    Title = "Wigilia",
                    TargetDate = new DateTime(2024, 12, 24),
                    UserId = 1,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = true,
                },

                new Reminder
                {
                    Id = 4,
                    Priority = 4,
                    Title = "Rocznica Odzyskania Niepodległości",
                    TargetDate = new DateTime(2024, 11, 11),
                    UserId = 2,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = true,
                },
                new Reminder
                {
                    Id = 5,
                    Priority = 4,
                    Title = "Święto Pracy",
                    TargetDate = new DateTime(2024, 05, 1),
                    UserId = 2,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = true,
                },

                new Reminder
                {
                    Id = 6,
                    Priority = 4,
                    Title = "Światowy Dzień Pizzy",
                    TargetDate = new DateTime(2024, 2, 9),
                    UserId = 3,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = true,
                },
                new Reminder
                {
                    Id = 7,
                    Priority = 4,
                    Title = "Fryzjer",
                    TargetDate = new DateTime(2024, 2, 28),
                    UserId = 1,
                    SecondsToNotify = UserSettings.GetDefaultSecondsToNotify(),
                    IsCyclic = false,
                }
            );
        }
    }
}
