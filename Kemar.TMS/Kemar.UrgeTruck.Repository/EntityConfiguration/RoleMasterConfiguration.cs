using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class RoleMasterConfiguration : IEntityTypeConfiguration<RoleMaster>
    {
        public void Configure(EntityTypeBuilder<RoleMaster> builder)
        {
            builder.HasKey(x => x.RoleId);
            builder.Property(x => x.RoleId).ValueGeneratedOnAdd();
            builder.Property(x => x.RoleName).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.RoleGroup).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
