using Kemar.TMS.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.TMS.Repository.EntityConfiguration
{
    public class TaskHistoryConfiguration : IEntityTypeConfiguration<TaskHistory>
    {
        public void Configure(EntityTypeBuilder<TaskHistory> builder)
        {
            builder.HasKey(x => x.TaskHistoryId);
            builder.Property(x => x.TaskHistoryId).ValueGeneratedOnAdd();
            builder.Property(x => x.TaskType).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.ExceptedStartDate).IsRequired(false);
            builder.Property(x => x.ExceptedEndDate).IsRequired(false);
            builder.Property(x => x.Status).IsRequired(false).HasMaxLength(50);
        }
    }
}
