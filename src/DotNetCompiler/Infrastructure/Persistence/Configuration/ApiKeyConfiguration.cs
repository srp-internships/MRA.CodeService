using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
    {
        public void Configure(EntityTypeBuilder<ApiKey> builder)
        {
            builder.HasKey(s => s.Guid);
            builder.Property(c => c.Guid).ValueGeneratedOnAdd();
            builder.Property(c => c.SecretKey).IsRequired();
            builder.HasIndex(s => s.SecretKey);
            builder.HasOne(s => s.Company).WithOne(s => s.ApiKey).HasForeignKey<ApiKey>(s => s.CompanyGuid);
        }
    }
}