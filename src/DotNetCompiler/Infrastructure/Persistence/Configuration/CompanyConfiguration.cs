using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(s => s.Guid);
            builder.Property(s => s.Name).IsRequired();
            builder.HasIndex(s => s.Name);
            builder.Property(s => s.Host).IsRequired();
            builder.HasIndex(s => s.Host);
           // builder.HasOne(s => s.ApiKey).WithOne().HasForeignKey<ApiKey>(s => s.CompanyGuid).IsRequired();
        }
    }
}
