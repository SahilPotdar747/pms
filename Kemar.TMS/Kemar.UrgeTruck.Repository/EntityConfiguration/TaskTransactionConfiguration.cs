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
    public class TaskTransactionConfiguration : IEntityTypeConfiguration<TaskTransaction>
    {
        public void Configure(EntityTypeBuilder<TaskTransaction> builder)
        {
            builder.HasKey(x => x.TaskId);
            builder.Property(x => x.TaskId).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).IsRequired(true);
            builder.Property(x => x.Description).IsRequired(true);
            builder.Property(x => x.AssignedBy).IsRequired(true);
            builder.HasOne(x => x.TaskTypeMaster).WithMany(x => x.TaskTransaction).HasForeignKey(x => x.TaskTypeId).IsRequired(true);
            builder.HasOne(x => x.ProjectMaster).WithMany(x => x.TaskTransaction).HasForeignKey(x => x.ProjectId).IsRequired(false);
            builder.HasOne(x => x.UserManager).WithMany(x => x.TaskTransaction).HasForeignKey(x => x.AssignedTo).IsRequired(false);
            
            builder.Property(x => x.Priority).IsRequired(true);
            builder.Property(x => x.AssignedDate).IsRequired(true);
            builder.Property(x => x.ExceptedStartDate).IsRequired(false);
            builder.Property(x => x.ExceptedEndDate).IsRequired(false);
            builder.Property(x => x.ActualStartDate).IsRequired(false);
            builder.Property(x => x.ActualEndDate).IsRequired(false);
            builder.Property(x => x.Status).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
            builder.Property(x => x.Remarks).IsRequired(false).HasMaxLength(150);
            builder.HasOne(x => x.DepartmentMaster).WithMany(x => x.TaskTransaction).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Restrict).IsRequired(false);
        }
    }
}
