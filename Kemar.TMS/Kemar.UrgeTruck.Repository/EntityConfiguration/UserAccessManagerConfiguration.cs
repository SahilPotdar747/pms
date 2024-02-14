using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class UserAccessManagerConfiguration : IEntityTypeConfiguration<UserAccessManager>
    {
        public void Configure(EntityTypeBuilder<UserAccessManager> builder)
        {
            builder.HasKey(x => x.UserAccessManagerId);
            builder.Property(x => x.UserAccessManagerId).ValueGeneratedOnAdd();

            builder.HasOne(x => x.RoleMaster)
                   .WithMany(x => x.UserAccessManager)
                   .HasForeignKey(x => x.RoleId)
                   .IsRequired(true);

            builder.HasOne(x => x.UserScreenMaster)
                   .WithMany(x => x.UserAccessManager)
                   .HasForeignKey(x => x.UserScreenId)
                   .IsRequired(true);
           
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
