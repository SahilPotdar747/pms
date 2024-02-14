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
    public class DelegateHistoryConfiguration : IEntityTypeConfiguration<DelegateHistory>
    {
        public void Configure(EntityTypeBuilder<DelegateHistory> builder)
        {
            builder.HasKey(x => x.delegateHistoryId);
            builder.Property(x => x.delegateHistoryId).ValueGeneratedOnAdd();
            builder.Property(x => x.RaisedBy).IsRequired(true);
            builder.HasOne(x => x.TaskTransaction).WithMany(x => x.DelegateHistory).HasForeignKey(x => x.TaskId).IsRequired(true);
            builder.Property(x => x.Status).IsRequired(true).HasMaxLength(100);
            builder.Property(x => x.Remarks).IsRequired(false).HasMaxLength(250);
            builder.Property(x => x.isActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
            builder.Property(x => x.RejectRemarks).IsRequired(false).HasMaxLength(150);
            builder.Property(x => x.TransferTo).IsRequired(true).HasMaxLength(50);
        }
    }
}
