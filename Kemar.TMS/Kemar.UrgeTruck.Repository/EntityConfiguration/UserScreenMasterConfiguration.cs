using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class UserScreenMasterConfiguration : IEntityTypeConfiguration<UserScreenMaster>
    {
        public void Configure(EntityTypeBuilder<UserScreenMaster> builder)
        {
            builder.HasKey(x => x.UserScreenId);
            builder.Property(x => x.UserScreenId).ValueGeneratedOnAdd();
            builder.Property(x => x.MenuName).IsRequired(true).HasMaxLength(60);
            builder.Property(x => x.ScreenName).IsRequired(true).HasMaxLength(60);
            builder.Property(x => x.ScreenCode).IsRequired(true).HasMaxLength(20);
            builder.Property(x => x.RoutingURL).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.MenuIcon).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
