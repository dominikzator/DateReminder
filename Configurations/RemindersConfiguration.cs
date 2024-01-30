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
            builder.Property(p => p.Title).HasMaxLength(20);
        }
    }
}
