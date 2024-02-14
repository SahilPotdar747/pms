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
    public class DesignationMasterConfiguration : IEntityTypeConfiguration<DesignationMaster>
    {
        public void Configure(EntityTypeBuilder<DesignationMaster> builder)
        {
            builder.HasKey(x => x.DesignationId);
            builder.Property(x => x.DesignationId).ValueGeneratedOnAdd();
            builder.Property(x => x.DesignationName).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
        }
    }
}
