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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(x => x.NotificationId);
            builder.Property(x => x.NotificationId).ValueGeneratedOnAdd();
            builder.HasOne(x => x.UserManager).WithMany(x => x.Notification).HasForeignKey(x => x.UserId).IsRequired(true);
            builder.Property(x => x.Title).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.Message).IsRequired(true).HasMaxLength(250);
            builder.Property(x => x.IsDeleted).HasDefaultValue(false);
            builder.Property(x => x.PushedToDesktop).HasDefaultValue(false);
            builder.Property(x => x.PushedToMobile).HasDefaultValue(false);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
        }
    }
}
