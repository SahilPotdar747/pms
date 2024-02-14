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
    public class TaskTypeMasterConfiguration : IEntityTypeConfiguration<TaskTypeMaster>
    {
        public void Configure(EntityTypeBuilder<TaskTypeMaster> builder)
        {
            builder.HasKey(x => x.TaskId);
            builder.Property(x => x.TaskId).ValueGeneratedOnAdd();
            builder.Property(x => x.TaskName).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.HasOne(x => x.DepartmentMaster).WithMany(x => x.TaskTypeMaster).HasForeignKey(x => x.DepartmentId).IsRequired(false);
            builder.Property(x => x.NextTaskId).IsRequired(false);
            builder.Property(x => x.NextTaskname).IsRequired(false);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
        }
    }
}
