using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateReminder.Configurations
{
    internal class UsersConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(p => p.Reminders).WithOne(p => p.User).HasForeignKey(p => p.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new User
                {
                    Id = 1,
                    UserName = "heniu123",
                    Password = "1234",
                    UserSettingsId = 1,
                },
                new User
                {
                    Id = 2,
                    UserName = "stasiu123",
                    Password = "1234",
                    UserSettingsId = 1,
                },
                new User
                {
                    Id = 3,
                    UserName = "mieciu321",
                    Password = "4321",
                    UserSettingsId = 1,
                }
            );
        }
    }
}
