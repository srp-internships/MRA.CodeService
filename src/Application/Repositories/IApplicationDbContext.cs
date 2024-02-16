using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IApplicationDbContext
    {
        Task<ApiKey> GetApiKeyAsync(string secretKey);

        Task AddNewEntity<T>(T apiKey) where T : class, IDbEntity;

        DbSet<T> GetEntities<T>() where T : class, IDbEntity;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
