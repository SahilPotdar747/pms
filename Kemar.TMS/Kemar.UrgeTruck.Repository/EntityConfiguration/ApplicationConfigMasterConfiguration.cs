using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class ApplicationConfigMasterConfiguration : IEntityTypeConfiguration<ApplicationConfigMaster>
    {
        public void Configure(EntityTypeBuilder<ApplicationConfigMaster> builder)
        {
            builder.HasKey(x => x.AppConfigId);
            builder.Property(x => x.AppConfigId).ValueGeneratedOnAdd();
            builder.Property(x => x.Key).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.Value).IsRequired(true).HasMaxLength(150);
            
            builder.Property(x => x.IsActive).HasDefaultValue(true);
            builder.Property(x => x.CreatedBy).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.ModifiedBy).IsRequired(false).HasMaxLength(30);
            builder.Property(x => x.CreatedDate).IsRequired(true);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
        }
    }
}
