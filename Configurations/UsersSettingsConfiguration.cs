using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DateReminder.Configurations
{
    internal class UsersSettingsConfiguration : IEntityTypeConfiguration<UserSettings>
    {
        public void Configure(EntityTypeBuilder<UserSettings> builder)
        {
            builder.HasIndex(x => x.Id).IsUnique();

            builder.HasMany(x => x.Users).WithOne(p => p.UserSettings).HasForeignKey(x => x.UserSettingsId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
