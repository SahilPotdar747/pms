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
    public class DepartmentMasterConfiguration : IEntityTypeConfiguration<DepartmentMaster>
    {
        public void Configure(EntityTypeBuilder<DepartmentMaster> builder)
        {
            builder.HasKey(x => x.DepartmentId);
            builder.Property(x => x.DepartmentId).ValueGeneratedOnAdd();
            builder.Property(x => x.DepartmentName).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
            builder.Property(x => x.coordinatingInchargeName).IsRequired(false).HasMaxLength(100);
        }
    }
}
