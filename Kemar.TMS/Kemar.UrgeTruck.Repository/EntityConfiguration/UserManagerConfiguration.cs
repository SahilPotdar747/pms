using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class UserManagerConfiguration : IEntityTypeConfiguration<UserManager>
    {
        public void Configure(EntityTypeBuilder<UserManager> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.UserName).HasMaxLength(20);
            builder.Property(x => x.FirstName).HasMaxLength(20);
            builder.Property(x => x.LastName).HasMaxLength(20);
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.Property(x => x.MobileNumber).HasMaxLength(20);
            builder.Property(x => x.AcceptTerms).HasMaxLength(1);

            builder.HasOne(x => x.RoleMaster)
                   .WithMany(y => y.UserManager)
                   .HasForeignKey(x => x.RoleId)
                   .IsRequired(true);

            builder.HasOne(x => x.DepartmentMaster)
                   .WithMany(y => y.UserManager)
                   .HasForeignKey(x => x.DepartmentId)
                   .IsRequired(true);

            builder.HasOne(x => x.DesignationMaster)
                   .WithMany(y => y.UserManager)
                   .HasForeignKey(x => x.DesignationId)
                   .IsRequired(true);

            builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true).HasColumnType("date");
            builder.Property(x => x.ModifiedDate).IsRequired(false).HasColumnType("date");
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.empCode).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.ProfileImagePath).IsRequired(false).HasMaxLength(250);
        }
    }
}
