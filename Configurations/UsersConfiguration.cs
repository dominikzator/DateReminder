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
        }
    }
}
