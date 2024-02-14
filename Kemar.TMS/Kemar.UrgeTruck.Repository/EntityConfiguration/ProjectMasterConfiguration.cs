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
    public class ProjectMasterConfiguration : IEntityTypeConfiguration<ProjectMaster>
    {
        public void Configure(EntityTypeBuilder<ProjectMaster> builder)
        {
            builder.HasKey(x => x.ProjectId);
            builder.Property(x => x.ProjectId).ValueGeneratedOnAdd();
            builder.Property(x => x.ProjectName).IsRequired(true).HasMaxLength(250);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(250);
            builder.Property(x => x.Remark).IsRequired(false).HasMaxLength(250);
            builder.HasOne(x => x.UserManager).WithMany(x => x.ProjectMaster).HasForeignKey(x => x.ManagerId).IsRequired(true);
            builder.Property(x => x.StartDate).IsRequired(true);
            builder.Property(x => x.EndDate).IsRequired(true);
            builder.Property(x => x.Status).IsRequired(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
        }
    }
}
