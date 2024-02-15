using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {

        }

        public Task<ApiKey> GetApiKeyAsync(string secretKey)
        {
            return Set<ApiKey>().FirstOrDefaultAsync(s => s.SecretKey == secretKey);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<T> GetEntities<T>() where T : class, IDbEntity
        {
            return Set<T>();
        }

        public async Task AddNewEntity<T>(T apiKey) where T : class, IDbEntity
        {
            await AddAsync(apiKey);
        }
    }
}
