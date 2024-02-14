using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class EmailConfigConfiguration : IEntityTypeConfiguration<EmailConfig>
    {
        public void Configure(EntityTypeBuilder<EmailConfig> builder)
        {
            builder.HasKey(x => x.EmailId);
            builder.Property(x => x.EmailId).ValueGeneratedOnAdd();
            builder.Property(x => x.Key).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.Body).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.To).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.CC).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.From).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}
