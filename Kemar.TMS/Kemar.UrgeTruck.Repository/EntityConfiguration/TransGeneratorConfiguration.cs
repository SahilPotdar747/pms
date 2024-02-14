
using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Kemar.UrgeTruck.Repository.EntityConfiguration
{
    public class TransGeneratorConfiguration : IEntityTypeConfiguration<TransGenerator>
    {
        public void Configure(EntityTypeBuilder<TransGenerator> builder)
        {
            builder.HasKey(x => x.TransactionIdKey);
            builder.Property(x => x.TransactionIdKey).ValueGeneratedOnAdd();
            builder.Property(x => x.TransactionType).IsRequired(true).HasMaxLength(30);
            builder.Property(x => x.Year).IsRequired(true);
            builder.Property(x => x.LastTransactionNumber);
        }
    }
}


